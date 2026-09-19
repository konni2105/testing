using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class CreateAttendanceViewModel
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        public bool IsPresent { get; set; }
    }
}