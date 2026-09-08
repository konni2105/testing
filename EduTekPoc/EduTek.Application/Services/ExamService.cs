using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _repository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;
        private readonly IClassSubjectRepository _classSubjectRepository;

        public ExamService(
            IExamRepository repository,
            ISubjectRepository subjectRepository,
            IClassRepository classRepository,
            IClassSubjectRepository classSubjectRepository)
        {
            _repository = repository;
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
            _classSubjectRepository = classSubjectRepository;
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
            // 1. Check Subject exists
            var subject =
                await _subjectRepository.GetByIdAsync(exam.SubjectId);

            if (subject == null)
            {
                throw new Exception("Subject not found.");
            }

            // 2. Check Class exists
            var classEntity =
                await _classRepository.GetByIdAsync(exam.ClassId);

            if (classEntity == null)
            {
                throw new Exception("Class not found.");
            }

            // 3. Check Subject is assigned to this Class
            var subjectAssigned =
                await _classSubjectRepository.ExistsAsync(
                    exam.ClassId,
                    exam.SubjectId);

            if (!subjectAssigned)
            {
                throw new Exception(
                    "Subject is not assigned to this class.");
            }

            // 4. Check duplicate exam
            var examExists =
                await _repository.ExistsAsync(
                    exam.SubjectId,
                    exam.ClassId,
                    exam.ExamDate);

            if (examExists)
            {
                throw new Exception(
                    "An exam already exists for this subject, class, and date.");
            }

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