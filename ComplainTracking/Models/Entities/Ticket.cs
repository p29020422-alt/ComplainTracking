using System.ComponentModel.DataAnnotations;
using ComplainTracking.Models.Entities.Enums;

namespace ComplainTracking.Models.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Category { get; set; } // Hardware, Software, Network, etc.

        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        public TicketStatus Status { get; set; } = TicketStatus.Opened;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ClosedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(500)]
        public string? AttachmentPath { get; set; }

        // Foreign keys
        [Required]
        public string SubmitterId { get; set; } = string.Empty;

        public string? AssignedAgentId { get; set; }

        // Navigation properties
        public ApplicationUser Submitter { get; set; } = null!;

        public ApplicationUser? AssignedAgent { get; set; }

        public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    }
}
