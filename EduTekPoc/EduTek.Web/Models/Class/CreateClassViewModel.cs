using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class CreateClassViewModel
    {
        [Required]
        [StringLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}