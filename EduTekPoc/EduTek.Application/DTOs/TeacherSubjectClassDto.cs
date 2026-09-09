public class TeacherSubjectClassDto
{
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;

    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;

    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
}