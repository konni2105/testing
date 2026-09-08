using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllAsync();

        Task<TeacherDto?> GetByIdAsync(int id);

        Task<TeacherDto> AddAsync(
            CreateTeacherDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateTeacherDto dto);

        Task<bool> DeleteAsync(int id);
    }
}