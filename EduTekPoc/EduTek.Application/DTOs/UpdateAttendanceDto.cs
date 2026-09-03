using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class UpdateAttendanceDto
    {
        [Required]
        public DateTime AttendanceDate { get; set; }

        [Required]
        public bool IsPresent { get; set; }
    }
}
