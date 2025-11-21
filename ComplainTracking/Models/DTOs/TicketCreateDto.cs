using System.ComponentModel.DataAnnotations;
using ComplainTracking.Models.Entities.Enums;

namespace ComplainTracking.Models.DTOs
{
    public class TicketCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Category { get; set; }

        [Required]
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        public IFormFile? Attachment { get; set; }
    }
}
