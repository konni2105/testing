using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class CreateFeedbackDto
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comments { get; set; } = string.Empty;

        [Required]
        public DateTime FeedbackDate { get; set; }
    }
}