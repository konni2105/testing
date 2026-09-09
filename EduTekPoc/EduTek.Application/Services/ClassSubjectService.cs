using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class ClassSubjectService : IClassSubjectService
    {
        private readonly IClassSubjectRepository _repository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;

        public ClassSubjectService(
            IClassSubjectRepository repository,
            IClassRepository classRepository,
            ISubjectRepository subjectRepository)
        {
            _repository = repository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<List<ClassSubjectDto>> GetAllAsync()
        {
            var classSubjects =
                await _repository.GetAllAsync();

            return classSubjects.Select(cs => new ClassSubjectDto
            {
                ClassId = cs.ClassId,
                ClassName = cs.Class.ClassName,
                SubjectId = cs.SubjectId,
                SubjectName = cs.Subject.SubjectName
            }).ToList();
        }

        public async Task<ClassSubjectDto?> GetAsync(
            int classId,
            int subjectId)
        {
            var classSubject =
                await _repository.GetAsync(
                    classId,
                    subjectId);

            if (classSubject == null)
                return null;

            return new ClassSubjectDto
            {
                ClassId = classSubject.ClassId,
                ClassName = classSubject.Class.ClassName,
                SubjectId = classSubject.SubjectId,
                SubjectName = classSubject.Subject.SubjectName
            };
        }

        public async Task<ClassSubjectDto> AddAsync(
            CreateClassSubjectDto dto)
        {
            var classEntity =
                await _classRepository.GetByIdAsync(dto.ClassId);

            if (classEntity == null)
                throw new Exception("Class not found.");

            var subject =
                await _subjectRepository.GetByIdAsync(dto.SubjectId);

            if (subject == null)
                throw new Exception("Subject not found.");

            var exists =
                await _repository.ExistsAsync(
                    dto.ClassId,
                    dto.SubjectId);

            if (exists)
                throw new Exception(
                    "This subject is already assigned to this class.");

            var classSubject = new ClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId
            };

            var created =
                await _repository.AddAsync(classSubject);

            return new ClassSubjectDto
            {
                ClassId = created.ClassId,
                ClassName = classEntity.ClassName,
                SubjectId = created.SubjectId,
                SubjectName = subject.SubjectName
            };
        }

        public async Task<bool> DeleteAsync(
            int classId,
            int subjectId)
        {
            return await _repository.DeleteAsync(
                classId,
                subjectId);
        }
    }
}