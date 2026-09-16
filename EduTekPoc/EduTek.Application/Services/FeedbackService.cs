using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherSubjectClassRepository _teacherSubjectClassRepository;
        private readonly ITeacherRepository _teacherRepository;

        public FeedbackService(
            IFeedbackRepository repository,
            IStudentRepository studentRepository,
            ITeacherSubjectClassRepository teacherSubjectClassRepository,
             ITeacherRepository teacherRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _teacherSubjectClassRepository = teacherSubjectClassRepository;
                 _teacherRepository = teacherRepository;
        }

        public async Task<List<FeedbackDto>> GetAllAsync()
        {
            var feedbacks = await _repository.GetAllAsync();

            return feedbacks.Select(f => new FeedbackDto
            {
                FeedbackId = f.FeedbackId,

                TeacherId = f.TeacherId,
                TeacherName =
                    $"{f.Teacher.FirstName} {f.Teacher.LastName}",

                StudentId = f.StudentId,
                StudentName =
                    $"{f.Student.FirstName} {f.Student.LastName}",

                Comments = f.Comments,
                FeedbackDate = f.FeedbackDate
            }).ToList();
        }

        public async Task<FeedbackDto?> GetByIdAsync(int id)
        {
            var feedback =
                await _repository.GetByIdAsync(id);

            if (feedback == null)
            {
                return null;
            }

            return new FeedbackDto
            {
                FeedbackId = feedback.FeedbackId,

                TeacherId = feedback.TeacherId,
                TeacherName =
                    $"{feedback.Teacher.FirstName} {feedback.Teacher.LastName}",

                StudentId = feedback.StudentId,
                StudentName =
                    $"{feedback.Student.FirstName} {feedback.Student.LastName}",

                Comments = feedback.Comments,
                FeedbackDate = feedback.FeedbackDate
            };
        }

        public async Task<FeedbackDto> AddAsync(
    CreateFeedbackDto dto)
        {
            // 1. Check Student exists
            var student =
                await _studentRepository.GetByIdAsync(
                    dto.StudentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            //checks teacher exists 
            var teacher =
                await _teacherRepository.GetByIdAsync(dto.TeacherId);

            if (teacher == null)
            {
                throw new Exception("Teacher not found.");
            }

            // 2. Check Teacher is assigned to student's class
            var teacherAssigned =
                await _teacherSubjectClassRepository
                    .ExistsByTeacherAndClassAsync(
                        dto.TeacherId,
                        student.ClassId);

            if (!teacherAssigned)
            {
                throw new Exception(
                    "Teacher is not assigned to this student's class.");
            }

            // 3. DTO → Entity
            var feedback = new Feedback
            {
                TeacherId = dto.TeacherId,
                StudentId = dto.StudentId,
                Comments = dto.Comments,
                FeedbackDate = dto.FeedbackDate
            };

            // 4. Save through Repository
            var created =
                await _repository.AddAsync(feedback);

            // 5. Entity → DTO
            return new FeedbackDto
            {
                FeedbackId = created.FeedbackId,

                TeacherId = created.TeacherId,
                TeacherName =
                    $"{teacher.FirstName} {teacher.LastName}",

                StudentId = created.StudentId,

                StudentName =
                    $"{student.FirstName} {student.LastName}",

                Comments = created.Comments,

                FeedbackDate = created.FeedbackDate
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateFeedbackDto dto)
        {
            var feedback =
                await _repository.GetByIdAsync(id);

            if (feedback == null)
            {
                return false;
            }

            feedback.Comments = dto.Comments;
            feedback.FeedbackDate = dto.FeedbackDate;

            return await _repository.UpdateAsync(
                id,
                feedback);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}