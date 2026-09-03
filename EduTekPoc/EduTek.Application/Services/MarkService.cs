using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _repository;

        public MarkService(IMarkRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Mark>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Mark?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Mark> AddAsync(Mark mark)
        {
            return await _repository.AddAsync(mark);
        }

        public async Task<bool> UpdateAsync(int id, Mark mark)
        {
            return await _repository.UpdateAsync(id, mark);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int examId, int studentId)
        {
            return await _repository.ExistsAsync(examId, studentId);
        }
    }
}