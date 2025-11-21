using System.ComponentModel.DataAnnotations;

namespace ComplainTracking.Models.Entities
{
    public class TicketComment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment content is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 1000 characters")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign keys
        public int TicketId { get; set; }

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        // Navigation properties
        public Ticket Ticket { get; set; } = null!;

        public ApplicationUser Author { get; set; } = null!;
    }
}
