using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class CreateClassDto
    {
        [Required]
        [StringLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}