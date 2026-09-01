using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllAsync();

        Task<Subject?> GetByIdAsync(int id);

        Task<Subject> AddAsync(Subject subject);

        Task<bool> UpdateAsync(int id, Subject subject);

        Task<bool> DeleteAsync(int id);
    }
}