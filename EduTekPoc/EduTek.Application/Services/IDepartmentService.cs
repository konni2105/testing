using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllAsync();

        Task<DepartmentDto?> GetByIdAsync(int id);

        Task<DepartmentDto> AddAsync(
            CreateDepartmentDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateDepartmentDto dto);

        Task<bool> DeleteAsync(int id);
    }
}