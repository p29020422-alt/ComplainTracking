using ComplainTracking.Models.DTOs;
using ComplainTracking.Models.Entities;
using ComplainTracking.Models.Entities.Enums;

namespace ComplainTracking.Core.Interfaces
{
    public interface ITicketService
    {
        Task<PaginatedResult<Ticket>> GetTicketsAsync(int page, int pageSize, string sort, string? userId = null);
        Task<Ticket?> GetTicketByIdAsync(int ticketId);
        Task CreateTicketAsync(Ticket ticket, IFormFile? attachment);
        Task AssignTicketAsync(int ticketId, string agentId);
        Task UpdateStatusAsync(int ticketId, TicketStatus status);
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int ticketId);
    }
}
