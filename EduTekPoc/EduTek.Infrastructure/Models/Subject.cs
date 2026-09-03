namespace EduTek.Infrastructure.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;

        public ICollection<ClassSubject> ClassSubjects { get; set; }
            = new List<ClassSubject>();

        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; }
            = new List<TeacherSubjectClass>();

        public ICollection<Attendance> Attendances { get; set; }
            = new List<Attendance>();

        public ICollection<Exam> Exams { get; set; }
            = new List<Exam>();

    }
}
 