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
            var exam = await _examRepository.GetByIdAsync(mark.ExamId);

            if (exam == null)
            {
                throw new Exception("Exam not found.");
            }

            var student = await _studentRepository.GetByIdAsync(mark.StudentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            if (exam.ClassId != student.ClassId)
            {
                throw new Exception(
                    "Student does not belong to the class for this exam.");
            }

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