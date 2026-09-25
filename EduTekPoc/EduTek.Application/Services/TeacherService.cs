using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(
            ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        // GET ALL
        public async Task<List<TeacherDto>> GetAllAsync()
        {
            var teachers =
                await _teacherRepository.GetAllAsync();

            return teachers.Select(t => new TeacherDto
            {
                TeacherId = t.TeacherId,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            }).ToList();
        }

        // GET BY ID
        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            var teacher =
                await _teacherRepository.GetByIdAsync(id);

            if (teacher == null)
                return null;

            return new TeacherDto
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber
            };
        }

        // CREATE
        public async Task<TeacherDto> AddAsync(
            CreateTeacherDto dto)
        {
            // Check duplicate email
            var emailExists =
                await _teacherRepository.EmailExistsAsync(
                    dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            // DTO ? Entity
            var teacher = new Teacher
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            // Save
            var createdTeacher =
                await _teacherRepository.AddAsync(teacher);

            // Entity ? DTO
            return new TeacherDto
            {
                TeacherId = createdTeacher.TeacherId,
                FirstName = createdTeacher.FirstName,
                LastName = createdTeacher.LastName,
                Email = createdTeacher.Email,
                PhoneNumber = createdTeacher.PhoneNumber
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id,
            UpdateTeacherDto dto)
        {
            // Check teacher exists
            var existingTeacher =
                await _teacherRepository.GetByIdAsync(id);

            if (existingTeacher == null)
                return false;

            // Check duplicate email
            var emailExists =
                await _teacherRepository
                    .EmailExistsForOtherTeacherAsync(
                        dto.Email,
                        id);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            // DTO ? Entity
            var teacher = new Teacher
            {
                TeacherId = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            return await _teacherRepository.UpdateAsync(
                id,
                teacher);
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            // Check teacher exists
            var teacher =
                await _teacherRepository.GetByIdAsync(id);

            if (teacher == null)
                return false;

            // Check whether teacher has assignments
            var hasAssignments =
                await _teacherRepository
                    .HasSubjectClassAssignmentsAsync(id);

            if (hasAssignments)
            {
                throw new InvalidOperationException(
                    "Cannot delete teacher - subject/class assignments exist. Deactivate instead.");
            }

            return await _teacherRepository.DeleteAsync(id);
        }
    }
}