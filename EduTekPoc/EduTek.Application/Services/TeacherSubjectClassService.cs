using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class TeacherSubjectClassService
        : ITeacherSubjectClassService
    {
        private readonly ITeacherSubjectClassRepository _repository;

        public TeacherSubjectClassService(
            ITeacherSubjectClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TeacherSubjectClass>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TeacherSubjectClass?> GetAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            return await _repository.GetAsync(
                teacherId,
                subjectId,
                classId);
        }

        public async Task<TeacherSubjectClass> AddAsync(
            TeacherSubjectClass assignment)
        {
            return await _repository.AddAsync(assignment);
        }

        public async Task<bool> DeleteAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            return await _repository.DeleteAsync(
                teacherId,
                subjectId,
                classId);
        }
    }
}