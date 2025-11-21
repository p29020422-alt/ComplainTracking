namespace ComplainTracking.Models.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ResolvedTickets { get; set; }
        public Dictionary<string, int> TicketsByCategory { get; set; } = new();
    }
}
