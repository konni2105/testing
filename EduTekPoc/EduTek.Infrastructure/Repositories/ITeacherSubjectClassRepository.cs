using EduTek.Infrastructure.Models;

public interface ITeacherSubjectClassRepository
{
    Task<List<TeacherSubjectClass>> GetAllAsync();

    Task<TeacherSubjectClass?> GetAsync(
        int teacherId,
        int subjectId,
        int classId);

    Task<TeacherSubjectClass> AddAsync(
        TeacherSubjectClass assignment);

    Task<bool> DeleteAsync(
        int teacherId,
        int subjectId,
        int classId);

    Task<bool> ExistsAsync(
        int teacherId,
        int subjectId,
        int classId);

    Task<bool> ExistsByTeacherAndClassAsync(
    int teacherId,
    int classId);
}