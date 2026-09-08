using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IAttendanceService
    {
        Task<List<AttendanceDto>> GetAllAsync();

        Task<AttendanceDto?> GetByIdAsync(int id);

        Task<AttendanceDto> AddAsync(
            CreateAttendanceDto dto);

        Task<bool> UpdateAsync(
            int id,
            UpdateAttendanceDto dto);

        Task<bool> DeleteAsync(int id);
    }
}