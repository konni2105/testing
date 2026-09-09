using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(
            IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        // GET ALL
        public async Task<List<ClassDto>> GetAllAsync()
        {
            var classes =
                await _classRepository.GetAllAsync();

            return classes.Select(c => new ClassDto
            {
                ClassId = c.ClassId,
                ClassName = c.ClassName,
                Description = c.Description
            }).ToList();
        }

        // GET BY ID
        public async Task<ClassDto?> GetByIdAsync(int id)
        {
            var classEntity =
                await _classRepository.GetByIdAsync(id);

            if (classEntity == null)
                return null;

            return new ClassDto
            {
                ClassId = classEntity.ClassId,
                ClassName = classEntity.ClassName,
                Description = classEntity.Description
            };
        }

        // CREATE
        public async Task<ClassDto> AddAsync(
            CreateClassDto dto)
        {
            var nameExists =
                await _classRepository
                    .NameExistsAsync(dto.ClassName);

            if (nameExists)
            {
                throw new Exception(
                    "Class name already exists.");
            }

            var classEntity = new Class
            {
                ClassName = dto.ClassName,
                Description = dto.Description
            };

            var createdClass =
                await _classRepository.AddAsync(classEntity);

            return new ClassDto
            {
                ClassId = createdClass.ClassId,
                ClassName = createdClass.ClassName,
                Description = createdClass.Description
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id,
            UpdateClassDto dto)
        {
            var existingClass =
                await _classRepository.GetByIdAsync(id);

            if (existingClass == null)
                return false;

            var nameExists =
                await _classRepository
                    .NameExistsForOtherClassAsync(
                        dto.ClassName,
                        id);

            if (nameExists)
            {
                throw new Exception(
                    "Class name already exists.");
            }

            var classEntity = new Class
            {
                ClassName = dto.ClassName,
                Description = dto.Description
            };

            return await _classRepository.UpdateAsync(
                id,
                classEntity);
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity =
                await _classRepository.GetByIdAsync(id);

            if (classEntity == null)
                return false;

            var hasStudents =
                await _classRepository.HasStudentsAsync(id);

            if (hasStudents)
            {
                throw new Exception(
                    "Cannot delete class because students are assigned to it.");
            }

            var hasSubjectAssignments =
                await _classRepository
                    .HasSubjectAssignmentsAsync(id);

            if (hasSubjectAssignments)
            {
                throw new Exception(
                    "Cannot delete class because subject assignments exist.");
            }

            var hasTeacherAssignments =
                await _classRepository
                    .HasTeacherAssignmentsAsync(id);

            if (hasTeacherAssignments)
            {
                throw new Exception(
                    "Cannot delete class because teacher assignments exist.");
            }

            var hasExams =
                await _classRepository.HasExamsAsync(id);

            if (hasExams)
            {
                throw new Exception(
                    "Cannot delete class because exams exist.");
            }

            return await _classRepository.DeleteAsync(id);
        }
    }
}