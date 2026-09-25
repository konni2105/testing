using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IMarkService
    {
        Task<List<MarkDto>> GetAllAsync();

        Task<MarkDto?> GetByIdAsync(int id);

        Task<MarkDto> AddAsync(CreateMarkDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateMarkDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(
            int examId,
            int studentId);
    }
}