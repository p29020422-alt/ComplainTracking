using ComplainTracking.Models.Entities;

namespace ComplainTracking.Core.Interfaces
{
    public interface ITicketCommentService
    {
        Task<List<TicketComment>> GetCommentsByTicketIdAsync(int ticketId);
        Task AddCommentAsync(TicketComment comment);
        Task UpdateCommentAsync(TicketComment comment);
        Task DeleteCommentAsync(int commentId);
    }
}
