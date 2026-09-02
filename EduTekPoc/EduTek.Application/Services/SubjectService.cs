using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _subjectRepository.GetAllAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _subjectRepository.GetByIdAsync(id);
        }

        public async Task<Subject> AddAsync(Subject subject)
        {
            return await _subjectRepository.AddAsync(subject);
        }

        public async Task<bool> UpdateAsync(
            int id,
            Subject subject)
        {
            return await _subjectRepository.UpdateAsync(id, subject);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _subjectRepository.DeleteAsync(id);
        }
    }
}