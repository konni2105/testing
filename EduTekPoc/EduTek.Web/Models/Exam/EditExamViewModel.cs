using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class EditExamViewModel
    {
        public int ExamId { get; set; }

        [Required]
        public string ExamName { get; set; } = string.Empty;

        [Required]
        public DateTime ExamDate { get; set; }
    }
}