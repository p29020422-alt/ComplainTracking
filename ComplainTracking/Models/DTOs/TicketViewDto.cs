using ComplainTracking.Models.Entities.Enums;

namespace ComplainTracking.Models.DTOs
{
    public class TicketViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? AttachmentPath { get; set; }

        // User Information
        public string SubmitterId { get; set; } = string.Empty;
        public string? SubmitterName { get; set; }
        public string? SubmitterEmail { get; set; }

        public string? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public string? AssignedAgentEmail { get; set; }

        // Comments Count
        public int CommentsCount { get; set; }
    }
}
