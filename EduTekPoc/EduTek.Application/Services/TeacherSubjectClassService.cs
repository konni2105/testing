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
            // 1. Check Teacher exists
            var teacher =
                await _teacherRepository.GetByIdAsync(
                    assignment.TeacherId);

            if (teacher == null)
            {
                throw new Exception("Teacher not found.");
            }


            // 2. Check Subject exists
            var subject =
                await _subjectRepository.GetByIdAsync(
                    assignment.SubjectId);

            if (subject == null)
            {
                throw new Exception("Subject not found.");
            }

            // 3. Check Class exists
            var classEntity =
                await _classRepository.GetByIdAsync(
                    assignment.ClassId);

            if (classEntity == null)
            {
                throw new Exception("Class not found.");
            }

            // 4. Check Subject is assigned to this Class
            var subjectAssigned =
                await _classSubjectRepository.ExistsAsync(
                    assignment.ClassId,
                    assignment.SubjectId);

            if (!subjectAssigned)
            {
                throw new Exception(
                    "Subject is not assigned to this class.");
            }

            // 5. Check duplicate assignment
            var assignmentExists =
                await _repository.ExistsAsync(
                    assignment.TeacherId,
                    assignment.SubjectId,
                    assignment.ClassId);

            if (assignmentExists)
            {
                throw new Exception(
                    "Teacher is already assigned to this subject and class.");
            }

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