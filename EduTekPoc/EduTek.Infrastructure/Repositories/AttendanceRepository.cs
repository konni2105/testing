using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);
        }

        public async Task<Attendance> AddAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);

            await _context.SaveChangesAsync();

            return attendance;
        }

        public async Task<bool> UpdateAsync(int id, Attendance attendance)
        {
            var existingAttendance =
                await _context.Attendances.FindAsync(id);

            if (existingAttendance == null)
            {
                return false;
            }

            existingAttendance.AttendanceDate =
                attendance.AttendanceDate;

            existingAttendance.IsPresent =
                attendance.IsPresent;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance =
                await _context.Attendances.FindAsync(id);

            if (attendance == null)
            {
                return false;
            }

            _context.Attendances.Remove(attendance);

            await _context.SaveChangesAsync();

            return true;
        }

        //Does this exact Teacher + Subject + Class assignment exist?
        public async Task<bool> IsTeacherAssignedAsync(
            int teacherId,
            int subjectId,
            int classId)
            {
                return await _context.TeacherSubjectClasses.AnyAsync(x =>
                    x.TeacherId == teacherId &&
                    x.SubjectId == subjectId &&
                    x.ClassId == classId);
        }

        //Does this Student actually belong to this Class?
        public async Task<bool> IsStudentInClassAsync(
        int studentId,
        int classId)
            {
            return await _context.Students.AnyAsync(x =>
                x.StudentId == studentId &&
                x.ClassId == classId);
        }
    }
}