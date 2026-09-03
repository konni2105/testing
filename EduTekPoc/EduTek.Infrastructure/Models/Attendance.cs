namespace EduTek.Infrastructure.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;

        public int SubjectId { get; set; }

        public Subject Subject { get; set; } = null!;

        public DateTime AttendanceDate { get; set; }

        public bool IsPresent { get; set; }
    }
}