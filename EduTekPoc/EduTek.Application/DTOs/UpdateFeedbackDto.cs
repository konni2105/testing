using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class UpdateFeedbackDto
    {
        [Required]
        [StringLength(1000)]
        public string Comments { get; set; } = string.Empty;

        [Required]
        public DateTime FeedbackDate { get; set; }
    }
}