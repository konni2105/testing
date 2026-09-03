namespace EduTek.Application.DTOs
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }

        public int TeacherId { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;

        public DateTime FeedbackDate { get; set; }
    }
}