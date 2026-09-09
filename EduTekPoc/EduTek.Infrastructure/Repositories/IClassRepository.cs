using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();

        Task<Class?> GetByIdAsync(int id);

        Task<Class> AddAsync(Class classEntity);

        Task<bool> UpdateAsync(
            int id,
            Class classEntity);

        Task<bool> DeleteAsync(int id);

        Task<bool> NameExistsAsync(string className);

        Task<bool> NameExistsForOtherClassAsync(
            string className,
            int classId);

        Task<bool> HasStudentsAsync(int classId);

        Task<bool> HasSubjectAssignmentsAsync(int classId);

        Task<bool> HasTeacherAssignmentsAsync(int classId);

        Task<bool> HasExamsAsync(int classId);
    }
}