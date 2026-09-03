namespace EduTek.Infrastructure.Models
{
    public class Mark
    {
        public int MarkId { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public decimal Score { get; set; }
    }
}