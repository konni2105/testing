using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class CreateClassSubjectDto
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}