using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _repository;

        public ExamService(IExamRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Exam>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Exam?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Exam> AddAsync(Exam exam)
        {
            return await _repository.AddAsync(exam);
        }

        public async Task<bool> UpdateAsync(int id, Exam exam)
        {
            return await _repository.UpdateAsync(id, exam);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}