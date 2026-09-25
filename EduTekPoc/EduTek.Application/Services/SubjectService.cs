using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace EduTek.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(
      ISubjectRepository subjectRepository,
      IMemoryCache cache,
      ILogger<SubjectService> logger)
        {
            _repository = subjectRepository;
            _cache = cache;
            _logger = logger;
        }

        // GET ALL
        public async Task<List<SubjectDto>> GetAllAsync()
        {
            const string cacheKey = "subjects";

            List<SubjectDto> subjects;

            if (_cache.TryGetValue(
                cacheKey,
                out List<SubjectDto>? cachedSubjects))
            {
                _logger.LogInformation("Subject GET cache HIT");
                subjects = cachedSubjects!;
            }
            else
            {
                _logger.LogInformation("Subject GET cache MISS");
                var result = await _repository.GetAllAsync();

                subjects = result.Select(s => new SubjectDto
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName,
                    Description = s.Description
                }).ToList();

                _cache.Set(
                    cacheKey,
                    subjects,
                    TimeSpan.FromMinutes(5));
            }

            return subjects;
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
                throw new InvalidOperationException(
                    "Subject name already exists.");

            var subject = new Subject
            {
                SubjectName = dto.SubjectName,
                Description = dto.Description,
                 DepartmentId = dto.DepartmentId
            };

            var createdSubject =
                await _repository.AddAsync(subject);

            _cache.Remove("subjects");

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
                throw new InvalidOperationException(
                    "Subject name already exists.");

            var subject = new Subject
            {
                SubjectName = dto.SubjectName,
                Description = dto.Description,
                
            };

           var result= await _repository.UpdateAsync(
                id,
                subject);


            if (result)
            {
                _cache.Remove("subjects");
            }

            return result;
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
                throw new InvalidOperationException(
                    "Cannot delete subject because it is assigned to a class.");
            }

            var hasTeacherAssignments =
                await _repository
                    .HasTeacherAssignmentsAsync(id);

            if (hasTeacherAssignments)
            {
                throw new InvalidOperationException(
                    "Cannot delete subject because teacher assignments exist.");
            }

            var hasExamRecords =
                await _repository
                    .HasExamRecordsAsync(id);

            if (hasExamRecords)
            {
                throw new InvalidOperationException(
                    "Cannot delete subject because exam records exist.");
            }

            var result = await _repository.DeleteAsync(id);

if (result)
{
    _cache.Remove("subjects");
}

return result;
        }
    }
}