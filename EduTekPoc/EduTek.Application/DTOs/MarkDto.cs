namespace EduTek.Application.DTOs
{
    public class MarkDto
    {
        public int MarkId { get; set; }

        public int ExamId { get; set; }

        public string ExamName { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public decimal Score { get; set; }
    }
}