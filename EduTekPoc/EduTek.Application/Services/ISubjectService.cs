using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface ISubjectService
    {
        Task<List<SubjectDto>> GetAllAsync();

        Task<SubjectDto?> GetByIdAsync(int id);

        Task<SubjectDto> AddAsync(CreateSubjectDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateSubjectDto dto);

        Task<bool> DeleteAsync(int id);
    }
}