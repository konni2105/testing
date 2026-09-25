using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IClassService
    {
        Task<List<ClassDto>> GetAllAsync();

        Task<ClassDto?> GetByIdAsync(int id);

        Task<ClassDto> AddAsync(CreateClassDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateClassDto dto);

        Task<bool> DeleteAsync(int id);
    }
}