using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class CreateExamViewModel
    {
        [Required]
        public string ExamName { get; set; } = string.Empty;

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }
    }
}