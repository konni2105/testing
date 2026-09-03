namespace EduTek.Infrastructure.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string Comments { get; set; } = string.Empty;

        public DateTime FeedbackDate { get; set; }
    }
}