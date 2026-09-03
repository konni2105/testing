namespace EduTek.Application.DTOs
{
    public class AttendanceDto
    {
        public int AttendanceId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;

        public DateTime AttendanceDate { get; set; }

        public bool IsPresent { get; set; }
    }
}