using EduTek.Application.DTOs;
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

        public async Task<List<ExamDto>> GetAllAsync()
        {
            var exams = await _repository.GetAllAsync();

            return exams.Select(e => new ExamDto
            {
                ExamId = e.ExamId,
                ExamName = e.ExamName,

                SubjectId = e.SubjectId,
                SubjectName = e.Subject.SubjectName,

                ClassId = e.ClassId,
                ClassName = e.Class.ClassName,

                ExamDate = e.ExamDate
            }).ToList();
        }

        public async Task<ExamDto?> GetByIdAsync(int id)
        {
            var exam = await _repository.GetByIdAsync(id);

            if (exam == null)
            {
                return null;
            }

            return new ExamDto
            {
                ExamId = exam.ExamId,
                ExamName = exam.ExamName,

                SubjectId = exam.SubjectId,
                SubjectName = exam.Subject.SubjectName,

                ClassId = exam.ClassId,
                ClassName = exam.Class.ClassName,

                ExamDate = exam.ExamDate
            };
        }

        public async Task<ExamDto> AddAsync(
            CreateExamDto dto)
        {
            // 1. Check Subject exists
            var subject =
                await _subjectRepository.GetByIdAsync(
                    dto.SubjectId);

            if (subject == null)
            {
                throw new Exception("Subject not found.");
            }

            // 2. Check Class exists
            var classEntity =
                await _classRepository.GetByIdAsync(
                    dto.ClassId);

            if (classEntity == null)
            {
                throw new Exception("Class not found.");
            }

            // 3. Check Subject is assigned to Class
            var subjectAssigned =
                await _classSubjectRepository.ExistsAsync(
                    dto.ClassId,
                    dto.SubjectId);

            if (!subjectAssigned)
            {
                throw new Exception(
                    "Subject is not assigned to this class.");
            }

            // 4. Check duplicate exam
            var examExists =
                await _repository.ExistsAsync(
                    dto.SubjectId,
                    dto.ClassId,
                    dto.ExamDate);

            if (examExists)
            {
                throw new Exception(
                    "An exam already exists for this subject, class, and date.");
            }

            // 5. DTO → Entity
            var exam = new Exam
            {
                ExamName = dto.ExamName,
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId,
                ExamDate = dto.ExamDate
            };

            // 6. Save through Repository
            var created =
                await _repository.AddAsync(exam);

            // 7. Entity → DTO
            return new ExamDto
            {
                ExamId = created.ExamId,
                ExamName = created.ExamName,

                SubjectId = created.SubjectId,
                SubjectName = subject.SubjectName,

                ClassId = created.ClassId,
                ClassName = classEntity.ClassName,

                ExamDate = created.ExamDate
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateExamDto dto)
        {
            var existingExam =
                await _repository.GetByIdAsync(id);

            if (existingExam == null)
            {
                return false;
            }

            var exam = new Exam
            {
                ExamName = dto.ExamName,
                ExamDate = dto.ExamDate
            };

            return await _repository.UpdateAsync(
                id,
                exam);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}