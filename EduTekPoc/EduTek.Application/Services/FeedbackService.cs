using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;

        public FeedbackService(IFeedbackRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Feedback> AddAsync(Feedback feedback)
        {
            return await _repository.AddAsync(feedback);
        }

        public async Task<bool> UpdateAsync(int id, Feedback feedback)
        {
            return await _repository.UpdateAsync(id, feedback);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}