using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repository;

        public SubjectService(
            ISubjectRepository repository)
        {
            _repository = repository;
        }

        // GET ALL
        public async Task<List<SubjectDto>> GetAllAsync()
        {
            var subjects =
                await _repository.GetAllAsync();

            return subjects.Select(s => new SubjectDto
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                Description = s.Description
            }).ToList();
        }

        // GET BY ID
        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            var subject =
                await _repository.GetByIdAsync(id);

            if (subject == null)
                return null;

            return new SubjectDto
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                Description = subject.Description
            };
        }

        // CREATE
        public async Task<SubjectDto> AddAsync(
            CreateSubjectDto dto)
        {
            var nameExists =
                await _repository.NameExistsAsync(
                    dto.SubjectName);

            if (nameExists)
                throw new Exception(
                    "Subject name already exists.");

            var subject = new Subject
            {
                SubjectName = dto.SubjectName,
                Description = dto.Description,
                 DepartmentId = dto.DepartmentId
            };

            var createdSubject =
                await _repository.AddAsync(subject);

            return new SubjectDto
            {
                SubjectId = createdSubject.SubjectId,
                SubjectName = createdSubject.SubjectName,
                Description = createdSubject.Description
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id,
            UpdateSubjectDto dto)
        {
            var existingSubject =
                await _repository.GetByIdAsync(id);

            if (existingSubject == null)
                return false;

            var nameExists =
                await _repository
                    .NameExistsForOtherSubjectAsync(
                        dto.SubjectName,
                        id);

            if (nameExists)
                throw new Exception(
                    "Subject name already exists.");

            var subject = new Subject
            {
                SubjectName = dto.SubjectName,
                Description = dto.Description,
                
            };

            return await _repository.UpdateAsync(
                id,
                subject);
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var subject =
                await _repository.GetByIdAsync(id);

            if (subject == null)
                return false;

            var hasClassAssignments =
                await _repository
                    .HasClassAssignmentsAsync(id);

            if (hasClassAssignments)
            {
                throw new Exception(
                    "Cannot delete subject because it is assigned to a class.");
            }

            var hasTeacherAssignments =
                await _repository
                    .HasTeacherAssignmentsAsync(id);

            if (hasTeacherAssignments)
            {
                throw new Exception(
                    "Cannot delete subject because teacher assignments exist.");
            }

            var hasExamRecords =
                await _repository
                    .HasExamRecordsAsync(id);

            if (hasExamRecords)
            {
                throw new Exception(
                    "Cannot delete subject because exam records exist.");
            }

            return await _repository.DeleteAsync(id);
        }
    }
}