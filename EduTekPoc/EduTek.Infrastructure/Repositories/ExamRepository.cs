using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Exam>> GetAllAsync()
        {
            return await _context.Exams
                .Include(e => e.Subject)
                .Include(e => e.Class)
                .ToListAsync();
        }

        public async Task<Exam?> GetByIdAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Subject)
                .Include(e => e.Class)
                .FirstOrDefaultAsync(e => e.ExamId == id);
        }

        public async Task<Exam> AddAsync(Exam exam)
        {
            _context.Exams.Add(exam);

            await _context.SaveChangesAsync();

            return exam;
        }

        public async Task<bool> UpdateAsync(int id, Exam exam)
        {
            var existingExam = await _context.Exams.FindAsync(id);

            if (existingExam == null)
            {
                return false;
            }

            existingExam.ExamName = exam.ExamName;
            existingExam.SubjectId = exam.SubjectId;
            existingExam.ClassId = exam.ClassId;
            existingExam.ExamDate = exam.ExamDate;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingExam = await _context.Exams.FindAsync(id);

            if (existingExam == null)
            {
                return false;
            }

            _context.Exams.Remove(existingExam);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}