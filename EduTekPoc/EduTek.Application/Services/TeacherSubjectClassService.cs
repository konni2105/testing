using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class TeacherSubjectClassService
        : ITeacherSubjectClassService
    {
        private readonly IClassSubjectRepository _classSubjectRepository;
        private readonly ITeacherSubjectClassRepository _repository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;

        public TeacherSubjectClassService(
            ITeacherSubjectClassRepository repository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IClassRepository classRepository,
            IClassSubjectRepository classSubjectRepository)
        {
            _repository = repository;
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
            _classSubjectRepository = classSubjectRepository;
        }

        public async Task<List<TeacherSubjectClassDto>> GetAllAsync()
        {
            var assignments = await _repository.GetAllAsync();

            return assignments.Select(x => new TeacherSubjectClassDto
            {
                TeacherId = x.TeacherId,
                TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",

                SubjectId = x.SubjectId,
                SubjectName = x.Subject.SubjectName,

                ClassId = x.ClassId,
                ClassName = x.Class.ClassName
            }).ToList();
        }

        public async Task<TeacherSubjectClassDto?> GetAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            var assignment = await _repository.GetAsync(
                teacherId,
                subjectId,
                classId);

            if (assignment == null)
            {
                return null;
            }

            return new TeacherSubjectClassDto
            {
                TeacherId = assignment.TeacherId,
                TeacherName =
                    $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}",

                SubjectId = assignment.SubjectId,
                SubjectName =
                    assignment.Subject.SubjectName,

                ClassId = assignment.ClassId,
                ClassName =
                    assignment.Class.ClassName
            };
        }

        public async Task<TeacherSubjectClassDto> AddAsync(
            CreateTeacherSubjectClassDto dto)
        {
            // 1. Check Teacher exists
            var teacher =
                await _teacherRepository.GetByIdAsync(dto.TeacherId);

            if (teacher == null)
            {
                throw new Exception("Teacher not found.");
            }

            // 2. Check Subject exists
            var subject =
                await _subjectRepository.GetByIdAsync(dto.SubjectId);

            if (subject == null)
            {
                throw new Exception("Subject not found.");
            }

            // 3. Check Class exists
            var classEntity =
                await _classRepository.GetByIdAsync(dto.ClassId);

            if (classEntity == null)
            {
                throw new Exception("Class not found.");
            }

            // 4. Check Subject is assigned to this Class
            var subjectAssigned =
                await _classSubjectRepository.ExistsAsync(
                    dto.ClassId,
                    dto.SubjectId);

            if (!subjectAssigned)
            {
                throw new Exception(
                    "Subject is not assigned to this class.");
            }

            // 5. Check duplicate assignment
            var assignmentExists =
                await _repository.ExistsAsync(
                    dto.TeacherId,
                    dto.SubjectId,
                    dto.ClassId);

            if (assignmentExists)
            {
                throw new Exception(
                    "Teacher is already assigned to this subject and class.");
            }

            // 6. DTO → Entity mapping
            var assignment = new TeacherSubjectClass
            {
                TeacherId = dto.TeacherId,
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId
            };

            // 7. Save through Repository
            var createdAssignment =
                await _repository.AddAsync(assignment);

            // 8. Entity → DTO mapping
            return new TeacherSubjectClassDto
            {
                TeacherId = createdAssignment.TeacherId,
                SubjectId = createdAssignment.SubjectId,
                ClassId = createdAssignment.ClassId,

                TeacherName =
                    $"{teacher.FirstName} {teacher.LastName}",

                SubjectName = subject.SubjectName,

                ClassName = classEntity.ClassName
            };
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

        public async Task<bool> ExistsAsync(
            int teacherId,
            int subjectId,
            int classId)
        {
            return await _repository.ExistsAsync(
                teacherId,
                subjectId,
                classId);
        }
    }
}