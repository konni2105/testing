using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class ClassSubjectService : IClassSubjectService
    {
        private readonly IClassSubjectRepository _repository;

        public ClassSubjectService(IClassSubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClassSubject>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ClassSubject?> GetAsync(
            int classId,
            int subjectId)
        {
            return await _repository.GetAsync(classId, subjectId);
        }

        public async Task<ClassSubject> AddAsync(ClassSubject classSubject)
        {
            return await _repository.AddAsync(classSubject);
        }

        public async Task<bool> DeleteAsync(
            int classId,
            int subjectId)
        {
            return await _repository.DeleteAsync(classId, subjectId);
        }
    }
}