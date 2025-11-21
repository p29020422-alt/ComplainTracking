using ComplainTracking.Core.Interfaces;
using ComplainTracking.Models.DTOs;
using ComplainTracking.Models.Entities;
using ComplainTracking.Models.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComplainTracking.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(
            ITicketService ticketService,
            UserManager<ApplicationUser> userManager,
            ILogger<TicketsController> logger)
        {
            _ticketService = ticketService;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int page = 1, string sort = "date", string? userId = null)
        {
            try
            {
                const int pageSize = 10;
                var user = await _userManager.GetUserAsync(User);
                
                // If user is not admin, only show their tickets
                if (!User.IsInRole("Admin") && string.IsNullOrEmpty(userId))
                {
                    userId = user?.Id;
                }

                var result = await _ticketService.GetTicketsAsync(page, pageSize, sort, userId);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tickets");
                TempData["Error"] = "An error occurred while retrieving tickets.";
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                
                // Check authorization
                if (!User.IsInRole("Admin") && 
                    ticket.SubmitterId != user?.Id && 
                    ticket.AssignedAgentId != user?.Id)
                {
                    return Forbid();
                }

                if (User.IsInRole("Admin"))
                {
                    ViewBag.Agents = await _userManager.GetUsersInRoleAsync("Agent");
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving ticket {id}");
                TempData["Error"] = "An error occurred while retrieving the ticket.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCreateDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return Unauthorized();
                    }

                    var ticket = new Ticket
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Category = model.Category,
                        Priority = model.Priority,
                        SubmitterId = user.Id,
                        Status = TicketStatus.Opened,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _ticketService.CreateTicketAsync(ticket, model.Attachment);
                    TempData["Success"] = $"Ticket created successfully. Ticket ID: {ticket.Id}";
                    return RedirectToAction(nameof(Details), new { id = ticket.Id });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the ticket.");
                return View(model);
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                
                // Only submitter and admin can edit
                if (!User.IsInRole("Admin") && ticket.SubmitterId != user?.Id)
                {
                    return Forbid();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving ticket {id} for edit");
                TempData["Error"] = "An error occurred while retrieving the ticket.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            try
            {
                var existingTicket = await _ticketService.GetTicketByIdAsync(id);
                if (existingTicket == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                
                // Only submitter and admin can edit
                if (!User.IsInRole("Admin") && existingTicket.SubmitterId != user?.Id)
                {
                    return Forbid();
                }

                // Preserve original values
                existingTicket.Title = ticket.Title;
                existingTicket.Description = ticket.Description;
                existingTicket.Category = ticket.Category;
                existingTicket.Priority = ticket.Priority;

                await _ticketService.UpdateTicketAsync(existingTicket);
                TempData["Success"] = "Ticket updated successfully.";
                return RedirectToAction(nameof(Details), new { id = ticket.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ticket {id}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the ticket.");
                return View(ticket);
            }
        }

        // POST: Tickets/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(id);
                TempData["Success"] = "Ticket deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ticket {id}");
                TempData["Error"] = "An error occurred while deleting the ticket.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tickets/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, TicketStatus status)
        {
            try
            {
                await _ticketService.UpdateStatusAsync(id, status);
                TempData["Success"] = "Ticket status updated successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ticket status for {id}");
                TempData["Error"] = "An error occurred while updating the ticket status.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // POST: Tickets/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int id, string agentId)
        {
            try
            {
                await _ticketService.AssignTicketAsync(id, agentId);
                TempData["Success"] = "Ticket assigned successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning ticket {id}");
                TempData["Error"] = "An error occurred while assigning the ticket.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // GET: Tickets/Dashboard
        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            if (User.Identity?.IsAuthenticated == true && !User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var stats = await _ticketService.GetDashboardStatsAsync();
                return View(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard stats");
                TempData["Error"] = "An error occurred while retrieving dashboard statistics.";
                return RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
