using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class CreateExamDto
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