using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetAllAsync();

        Task<Feedback?> GetByIdAsync(int id);

        Task<Feedback> AddAsync(Feedback feedback);

        Task<bool> UpdateAsync(int id, Feedback feedback);

        Task<bool> DeleteAsync(int id);
    }
}