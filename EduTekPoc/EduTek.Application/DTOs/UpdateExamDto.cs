using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class UpdateExamDto
    {
        [Required]
        public string ExamName { get; set; } = string.Empty;

        [Required]
        public DateTime ExamDate { get; set; }
    }
}