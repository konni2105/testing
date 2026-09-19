using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class EditAttendanceViewModel
    {
        public int AttendanceId { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        [Required]
        public bool IsPresent { get; set; }
    }
}