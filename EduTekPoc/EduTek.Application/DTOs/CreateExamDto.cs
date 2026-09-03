using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class CreateExamDto
    {
        [Required]
        [StringLength(100)]
        public string ExamName { get; set; } = string.Empty;

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }
    }
}