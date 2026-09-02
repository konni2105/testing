namespace EduTek.Infrastructure.Models
{
    public class Class
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Student> Students { get; set; }
            = new List<Student>();

        public ICollection<ClassSubject> ClassSubjects { get; set; }
            = new List<ClassSubject>();

        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; }
            = new List<TeacherSubjectClass>();
    }
}
