using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllAsync();

        Task<Feedback?> GetByIdAsync(int id);

        Task<Feedback> AddAsync(Feedback feedback);

        Task<bool> UpdateAsync(int id, Feedback feedback);

        Task<bool> DeleteAsync(int id);
    }
}