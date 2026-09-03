using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class ClassSubjectRepository : IClassSubjectRepository
    {
        private readonly AppDbContext _context;

        public ClassSubjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassSubject>> GetAllAsync()
        {
            return await _context.ClassSubjects
                .Include(cs => cs.Class)
                .Include(cs => cs.Subject)
                .ToListAsync();
        }

        public async Task<ClassSubject?> GetAsync(int classId, int subjectId)
        {
            return await _context.ClassSubjects
                .FirstOrDefaultAsync(cs =>
                    cs.ClassId == classId &&
                    cs.SubjectId == subjectId);
        }

        public async Task<ClassSubject> AddAsync(ClassSubject classSubject)
        {
            _context.ClassSubjects.Add(classSubject);
            await _context.SaveChangesAsync();

            return classSubject;
        }

        public async Task<bool> DeleteAsync(int classId, int subjectId)
        {
            var existing = await GetAsync(classId, subjectId);

            if (existing == null)
            {
                return false;
            }

            _context.ClassSubjects.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}