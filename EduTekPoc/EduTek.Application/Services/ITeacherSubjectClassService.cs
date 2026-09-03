using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface ITeacherSubjectClassService
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
    }
}
