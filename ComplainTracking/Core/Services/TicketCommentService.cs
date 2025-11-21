using ComplainTracking.Core.Interfaces;
using ComplainTracking.Data;
using ComplainTracking.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComplainTracking.Core.Services
{
    public class TicketCommentService : ITicketCommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TicketCommentService> _logger;

        public TicketCommentService(ApplicationDbContext context, ILogger<TicketCommentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TicketComment>> GetCommentsByTicketIdAsync(int ticketId)
        {
            try
            {
                return await _context.TicketComments
                    .Where(c => c.TicketId == ticketId)
                    .Include(c => c.Author)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving comments for ticket {ticketId}");
                throw;
            }
        }

        public async Task AddCommentAsync(TicketComment comment)
        {
            try
            {
                _context.TicketComments.Add(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment added to ticket {comment.TicketId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding comment to ticket {comment.TicketId}");
                throw;
            }
        }

        public async Task UpdateCommentAsync(TicketComment comment)
        {
            try
            {
                comment.UpdatedAt = DateTime.UtcNow;
                _context.TicketComments.Update(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment {comment.Id} updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating comment {comment.Id}");
                throw;
            }
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            try
            {
                var comment = await _context.TicketComments.FindAsync(commentId);
                if (comment == null)
                {
                    throw new InvalidOperationException($"Comment with ID {commentId} not found");
                }

                _context.TicketComments.Remove(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment {commentId} deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting comment {commentId}");
                throw;
            }
        }
    }
}
