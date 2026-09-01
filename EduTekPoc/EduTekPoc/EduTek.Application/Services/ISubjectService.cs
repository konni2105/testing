using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetAllAsync();

        Task<Subject?> GetByIdAsync(int id);

        Task<Subject> AddAsync(Subject subject);

        Task<bool> UpdateAsync(int id, Subject subject);

        Task<bool> DeleteAsync(int id);
    }
}