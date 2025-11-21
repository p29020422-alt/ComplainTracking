using ComplainTracking.Core.Interfaces;
using ComplainTracking.Data;
using ComplainTracking.Models.DTOs;
using ComplainTracking.Models.Entities;
using ComplainTracking.Models.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace ComplainTracking.Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TicketService> _logger;

        public TicketService(
            ApplicationDbContext context,
            IEmailService emailService,
            IWebHostEnvironment env,
            ILogger<TicketService> logger)
        {
            _context = context;
            _emailService = emailService;
            _env = env;
            _logger = logger;
        }

        public async Task<PaginatedResult<Ticket>> GetTicketsAsync(int page, int pageSize, string sort, string? userId = null)
        {
            var query = _context.Tickets.AsQueryable();

            // Filter by user if provided
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.SubmitterId == userId || t.AssignedAgentId == userId);
            }

            // Apply sorting
            query = sort?.ToLower() switch
            {
                "priority" => query.OrderByDescending(t => t.Priority),
                "date" => query.OrderByDescending(t => t.CreatedAt),
                "status" => query.OrderBy(t => t.Status),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(t => t.Submitter)
                .Include(t => t.AssignedAgent)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Ticket>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<Ticket?> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets
                .Include(t => t.Submitter)
                .Include(t => t.AssignedAgent)
                .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task CreateTicketAsync(Ticket ticket, IFormFile? attachment)
        {
            try
            {
                if (attachment != null && attachment.Length > 0)
                {
                    // Ensure uploads directory exists
                    var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
                    var path = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await attachment.CopyToAsync(stream);
                    }

                    ticket.AttachmentPath = "/uploads/" + fileName;
                    _logger.LogInformation($"File uploaded: {fileName}");
                }

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Ticket created with ID: {ticket.Id}");

                // Notify Admin (Mock)
                await _emailService.SendEmailAsync(
                    "admin@system.com",
                    "New Ticket Created",
                    $"Ticket #{ticket.Id}: {ticket.Title} has been created by {ticket.Submitter?.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket");
                throw;
            }
        }

        public async Task AssignTicketAsync(int ticketId, string agentId)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);
                if (ticket == null)
                {
                    throw new InvalidOperationException($"Ticket with ID {ticketId} not found");
                }

                var agent = await _context.Users.FindAsync(agentId);
                if (agent == null)
                {
                    throw new InvalidOperationException($"Agent with ID {agentId} not found");
                }

                ticket.AssignedAgentId = agentId;
                ticket.Status = TicketStatus.Assigned;
                ticket.UpdatedAt = DateTime.UtcNow;

                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Ticket {ticketId} assigned to agent {agentId}");

                // Notify Agent (Mock)
                await _emailService.SendEmailAsync(
                    agent.Email ?? string.Empty,
                    "Ticket Assigned",
                    $"Ticket #{ticket.Id}: {ticket.Title} has been assigned to you");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning ticket {ticketId}");
                throw;
            }
        }

        public async Task UpdateStatusAsync(int ticketId, TicketStatus status)
        {
            try
            {
                var ticket = await _context.Tickets.Include(t => t.Submitter).FirstOrDefaultAsync(t => t.Id == ticketId);
                if (ticket == null)
                {
                    throw new InvalidOperationException($"Ticket with ID {ticketId} not found");
                }

                ticket.Status = status;
                ticket.UpdatedAt = DateTime.UtcNow;

                if (status == TicketStatus.Closed)
                {
                    ticket.ClosedAt = DateTime.UtcNow;
                }

                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Ticket {ticketId} status updated to {status}");

                // Notify Submitter (Mock)
                await _emailService.SendEmailAsync(
                    ticket.Submitter?.Email ?? string.Empty,
                    $"Ticket Status Updated",
                    $"Ticket #{ticket.Id}: {ticket.Title} status has been updated to {status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ticket status for {ticketId}");
                throw;
            }
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            try
            {
                var categoryStats = await _context.Tickets
                    .GroupBy(t => t.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToListAsync();

                var stats = new DashboardStatsDto
                {
                    TotalTickets = await _context.Tickets.CountAsync(),
                    OpenTickets = await _context.Tickets.CountAsync(t => t.Status == TicketStatus.Opened),
                    ResolvedTickets = await _context.Tickets.CountAsync(t => t.Status == TicketStatus.Resolved),
                    TicketsByCategory = categoryStats
                        .Where(x => !string.IsNullOrEmpty(x.Category))
                        .ToDictionary(k => k.Category!, v => v.Count)
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard stats");
                throw;
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.UpdatedAt = DateTime.UtcNow;
                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Ticket {ticket.Id} updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ticket {ticket.Id}");
                throw;
            }
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);
                if (ticket == null)
                {
                    throw new InvalidOperationException($"Ticket with ID {ticketId} not found");
                }

                // Delete attachment if exists
                if (!string.IsNullOrEmpty(ticket.AttachmentPath))
                {
                    var filePath = Path.Combine(_env.WebRootPath, ticket.AttachmentPath.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        _logger.LogInformation($"File deleted: {ticket.AttachmentPath}");
                    }
                }

                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Ticket {ticketId} deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ticket {ticketId}");
                throw;
            }
        }
    }
}
