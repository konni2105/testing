using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IExamService
    {
        Task<List<ExamDto>> GetAllAsync();

        Task<ExamDto?> GetByIdAsync(int id);

        Task<ExamDto> AddAsync(
            CreateExamDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateExamDto dto);

        Task<bool> DeleteAsync(int id);
    }
}