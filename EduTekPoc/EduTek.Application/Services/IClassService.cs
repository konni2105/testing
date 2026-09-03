using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IClassService
    {
        Task<List<Class>> GetAllAsync();

        Task<Class?> GetByIdAsync(int id);

        Task<Class> AddAsync(Class classEntity);

        Task<bool> UpdateAsync(int id, Class classEntity);

        Task<bool> DeleteAsync(int id);
    }
}