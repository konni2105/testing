using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class EditSubjectViewModel
    {
        public int SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}