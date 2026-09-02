using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllAsync();

        Task<StudentDto?> GetByIdAsync(int id);

        Task<StudentDto> CreateAsync(CreateStudentDto dto);

        Task<bool> UpdateAsync(int id, UpdateStudentDto dto);

        Task<bool> DeleteAsync(int id);
    }
}