using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}