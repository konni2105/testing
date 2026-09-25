using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _repository;
        private readonly IExamRepository _examRepository;
        private readonly IStudentRepository _studentRepository;

        public MarkService(
            IMarkRepository repository,
            IExamRepository examRepository,
            IStudentRepository studentRepository)
        {
            _repository = repository;
            _examRepository = examRepository;
            _studentRepository = studentRepository;
        }

        public async Task<List<MarkDto>> GetAllAsync()
        {
            var marks = await _repository.GetAllAsync();

            return marks.Select(m => new MarkDto
            {
                MarkId = m.MarkId,

                ExamId = m.ExamId,
                ExamName = m.Exam.ExamName,

                StudentId = m.StudentId,
                StudentName =
                    $"{m.Student.FirstName} {m.Student.LastName}",

                Score = m.Score

            }).ToList();
        }

        public async Task<MarkDto?> GetByIdAsync(int id)
        {
            var mark = await _repository.GetByIdAsync(id);

            if (mark == null)
            {
                return null;
            }

            return new MarkDto
            {
                MarkId = mark.MarkId,

                ExamId = mark.ExamId,
                ExamName = mark.Exam.ExamName,

                StudentId = mark.StudentId,
                StudentName =
                    $"{mark.Student.FirstName} {mark.Student.LastName}",

                Score = mark.Score
            };
        }

        public async Task<MarkDto> AddAsync(
            CreateMarkDto dto)
        {
            // 1. Check Exam exists
            var exam =
                await _examRepository.GetByIdAsync(
                    dto.ExamId);

            if (exam == null)
            {
                throw new InvalidOperationException("Exam not found.");
            }

            // 2. Check Student exists
            var student =
                await _studentRepository.GetByIdAsync(
                    dto.StudentId);

            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            // 3. Check Student belongs to Exam's class
            if (exam.ClassId != student.ClassId)
            {
                throw new InvalidOperationException(
                    "Student does not belong to the class for this exam.");
            }

            // 4. Check duplicate mark
            var markExists =
                await _repository.ExistsAsync(
                    dto.ExamId,
                    dto.StudentId);

            if (markExists)
            {
                throw new InvalidOperationException(
                    "A mark already exists for this student and exam.");
            }

            // 5. DTO ? Entity
            var mark = new Mark
            {
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                Score = dto.Score
            };

            // 6. Save through Repository
            var created =
                await _repository.AddAsync(mark);

            // 7. Entity ? DTO
            return new MarkDto
            {
                MarkId = created.MarkId,

                ExamId = created.ExamId,
                ExamName = exam.ExamName,

                StudentId = created.StudentId,
                StudentName =
                    $"{student.FirstName} {student.LastName}",

                Score = created.Score
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateMarkDto dto)
        {
            var mark =
                new Mark
                {
                    Score = dto.Score
                };

            return await _repository.UpdateAsync(
                id,
                mark);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(
            int examId,
            int studentId)
        {
            return await _repository.ExistsAsync(
                examId,
                studentId);
        }
    }
}