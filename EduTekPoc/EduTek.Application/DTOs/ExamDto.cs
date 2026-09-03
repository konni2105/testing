namespace EduTek.Application.DTOs
{
    public class ExamDto
    {
        public int ExamId { get; set; }

        public string ExamName { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;

        public int ClassId { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public DateTime ExamDate { get; set; }
    }
}