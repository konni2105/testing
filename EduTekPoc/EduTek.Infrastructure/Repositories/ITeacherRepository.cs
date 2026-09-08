using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetAllAsync();

        Task<Teacher?> GetByIdAsync(int id);

        Task<Teacher> AddAsync(
            Teacher teacher);

        Task<bool> UpdateAsync(
            int id,
            Teacher teacher);

        Task<bool> DeleteAsync(int id);

        Task<bool> EmailExistsAsync(
            string email);

        Task<bool> EmailExistsForOtherTeacherAsync(
            string email,
            int teacherId);

        Task<bool> HasSubjectClassAssignmentsAsync(
            int teacherId);
    }
}