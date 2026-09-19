using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class EditFeedbackViewModel
    {
        public int FeedbackId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comments { get; set; } = string.Empty;

        [Required]
        public DateTime FeedbackDate { get; set; }
    }
}