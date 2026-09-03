using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        private readonly AppDbContext _context;

        public MarkRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mark>> GetAllAsync()
        {
            return await _context.Marks
                .Include(m => m.Exam)
                .Include(m => m.Student)
                .ToListAsync();
        }

        public async Task<Mark?> GetByIdAsync(int id)
        {
            return await _context.Marks
                .Include(m => m.Exam)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.MarkId == id);
        }

        public async Task<Mark> AddAsync(Mark mark)
        {
            _context.Marks.Add(mark);

            await _context.SaveChangesAsync();

            return mark;
        }

        public async Task<bool> UpdateAsync(int id, Mark mark)
        {
            var existingMark = await _context.Marks.FindAsync(id);

            if (existingMark == null)
            {
                return false;
            }

            existingMark.Score = mark.Score;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingMark = await _context.Marks.FindAsync(id);

            if (existingMark == null)
            {
                return false;
            }

            _context.Marks.Remove(existingMark);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int examId, int studentId)
        {
            return await _context.Marks.AnyAsync(m =>
                m.ExamId == examId &&
                m.StudentId == studentId);
        }
    }
}