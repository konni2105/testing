using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repository;

        public AttendanceService(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Attendance>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Attendance> AddAsync(
            Attendance attendance)
        {
            return await _repository.AddAsync(attendance);
        }

        public async Task<bool> UpdateAsync(
            int id,
            Attendance attendance)
        {
            return await _repository.UpdateAsync(id, attendance);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> IsTeacherAssignedAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            return await _repository.IsTeacherAssignedAsync(
                teacherId,
                subjectId,
                classId);
        }

        public async Task<bool> IsStudentInClassAsync(
            int studentId,
            int classId)
        {
            return await _repository.IsStudentInClassAsync(
                studentId,
                classId);
        }
    }
}