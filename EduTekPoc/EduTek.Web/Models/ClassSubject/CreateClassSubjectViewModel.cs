using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class CreateClassSubjectViewModel
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}