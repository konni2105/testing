namespace EduTek.Infrastructure.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        public string ExamName { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public Subject Subject { get; set; } = null!;

        public int ClassId { get; set; }

        public Class Class { get; set; } = null!;

        public DateTime ExamDate { get; set; }

        public ICollection<Mark> Marks { get; set; }
            = new List<Mark>();
    }
}