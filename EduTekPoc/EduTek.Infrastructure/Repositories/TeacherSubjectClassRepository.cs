using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class TeacherSubjectClassRepository
        : ITeacherSubjectClassRepository
    {
        private readonly AppDbContext _context;

        public TeacherSubjectClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeacherSubjectClass>> GetAllAsync()
        {
            return await _context.TeacherSubjectClasses
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Include(x => x.Class)
                .ToListAsync();
        }

        public async Task<TeacherSubjectClass?> GetAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            return await _context.TeacherSubjectClasses
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Include(x => x.Class)
                .FirstOrDefaultAsync(x =>
                    x.TeacherId == teacherId &&
                    x.SubjectId == subjectId &&
                    x.ClassId == classId);
        }

        public async Task<TeacherSubjectClass> AddAsync(
            TeacherSubjectClass assignment)
        {
            _context.TeacherSubjectClasses.Add(assignment);

            await _context.SaveChangesAsync();

            return assignment;
        }

        public async Task<bool> DeleteAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            var assignment = await _context.TeacherSubjectClasses
                .FirstOrDefaultAsync(x =>
                    x.TeacherId == teacherId &&
                    x.SubjectId == subjectId &&
                    x.ClassId == classId);

            if (assignment == null)
            {
                return false;
            }

            _context.TeacherSubjectClasses.Remove(assignment);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
