using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllAsync();

        Task<Subject?> GetByIdAsync(int id);

        Task<Subject> AddAsync(Subject subject);

        Task<bool> UpdateAsync(
            int id,
            Subject subject);

        Task<bool> DeleteAsync(int id);

        Task<bool> NameExistsAsync(string subjectName);

        Task<bool> NameExistsForOtherSubjectAsync(
            string subjectName,
            int subjectId);

        Task<bool> HasClassAssignmentsAsync(int subjectId);

        Task<bool> HasTeacherAssignmentsAsync(int subjectId);

        Task<bool> HasExamRecordsAsync(int subjectId);
    }
}