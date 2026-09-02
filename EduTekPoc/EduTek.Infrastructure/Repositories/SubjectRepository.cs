using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;

        public SubjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        public async Task<Subject> AddAsync(Subject subject)
        {
            _context.Subjects.Add(subject);

            await _context.SaveChangesAsync();

            return subject;
        }

        public async Task<bool> UpdateAsync(int id, Subject subject)
        {
            var existingSubject = await _context.Subjects.FindAsync(id);

            if (existingSubject == null)
            {
                return false;
            }

            existingSubject.SubjectName = subject.SubjectName;
            existingSubject.Description = subject.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                return false;
            }

            _context.Subjects.Remove(subject);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
