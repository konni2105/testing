using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAllAsync();

        Task<Attendance?> GetByIdAsync(int id);

        Task<Attendance> AddAsync(Attendance attendance);

        Task<bool> UpdateAsync(int id, Attendance attendance);

        Task<bool> DeleteAsync(int id);

        Task<bool> IsTeacherAssignedAsync(
            int teacherId,
            int subjectId,
            int classId);

        Task<bool> IsStudentInClassAsync(
            int studentId,
            int classId);
    }
}