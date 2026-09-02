using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department> AddAsync(Department department);

        Task<bool> UpdateAsync(int id, Department department);

        Task<bool> DeleteAsync(int id);
    }
}
