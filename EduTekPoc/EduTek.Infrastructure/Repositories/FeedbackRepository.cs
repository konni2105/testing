using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.Teacher)
                .Include(f => f.Student)
                .ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks
                .Include(f => f.Teacher)
                .Include(f => f.Student)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);
        }

        public async Task<Feedback> AddAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);

            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<bool> UpdateAsync(int id, Feedback feedback)
        {
            var existingFeedback =
                await _context.Feedbacks.FindAsync(id);

            if (existingFeedback == null)
            {
                return false;
            }

            existingFeedback.Comments = feedback.Comments;
            existingFeedback.FeedbackDate = feedback.FeedbackDate;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingFeedback =
                await _context.Feedbacks.FindAsync(id);

            if (existingFeedback == null)
            {
                return false;
            }

            _context.Feedbacks.Remove(existingFeedback);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}