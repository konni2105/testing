using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IClassRepository _classRepository;

        public StudentService(
            IStudentRepository repository,
            IClassRepository classRepository)
        {
            _repository = repository;
            _classRepository = classRepository;
        }

        // GET ALL
        public async Task<List<StudentDto>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();

            return students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                DateOfBirth = s.DateOfBirth,
                ClassId = s.ClassId
            }).ToList();
        }

        // GET BY ID
        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                return null;

            return new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                DateOfBirth = student.DateOfBirth,
                ClassId = student.ClassId
            };
        }

        // CREATE
        public async Task<StudentDto> CreateAsync(
            CreateStudentDto dto)
        {
            // Check whether class exists
            var classEntity =
                await _classRepository.GetByIdAsync(dto.ClassId);

            if (classEntity == null)
                throw new InvalidOperationException("Class not found.");

            // Check duplicate email
            var emailExists =
                await _repository.EmailExistsAsync(dto.Email);

            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            // DTO ? Entity
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                ClassId = dto.ClassId
            };

            // Save to database
            var createdStudent =
                await _repository.AddAsync(student);

            // Entity ? DTO
            return new StudentDto
            {
                StudentId = createdStudent.StudentId,
                FirstName = createdStudent.FirstName,
                LastName = createdStudent.LastName,
                Email = createdStudent.Email,
                PhoneNumber = createdStudent.PhoneNumber,
                DateOfBirth = createdStudent.DateOfBirth,
                ClassId = createdStudent.ClassId
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id,
            UpdateStudentDto dto)
        {
            // Check student exists
            var existingStudent =
                await _repository.GetByIdAsync(id);

            if (existingStudent == null)
                return false;

            // Check class exists
            var classEntity =
                await _classRepository.GetByIdAsync(dto.ClassId);

            if (classEntity == null)
                throw new InvalidOperationException("Class not found.");

            // Check email belongs to another student
            var emailExists =
                await _repository.EmailExistsForOtherStudentAsync(
                    dto.Email,
                    id);

            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            // DTO ? Entity
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                ClassId = dto.ClassId
            };

            return await _repository.UpdateAsync(id, student);
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            // Check student exists
            var student =
                await _repository.GetByIdAsync(id);

            if (student == null)
                return false;

            // Check whether attendance records exist
            var hasAttendance =
                await _repository.HasAttendanceAsync(id);

            if (hasAttendance)
            {
                throw new InvalidOperationException(
                    "Cannot delete student - Attendance records exist. Deactivate instead.");
            }

            return await _repository.DeleteAsync(id);
        }
    }
}