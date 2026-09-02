namespace EduTek.Infrastructure.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; }
        = new List<TeacherSubjectClass>();
    }
}
