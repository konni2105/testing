using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }


        //entity to dto
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
                DateOfBirth = s.DateOfBirth
            }).ToList();
        }

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
                DateOfBirth = student.DateOfBirth
            };
        }


        //post -> dto to entity
        public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth
            };

            var createdStudent = await _repository.AddAsync(student);

            return new StudentDto
            {
                StudentId = createdStudent.StudentId,
                FirstName = createdStudent.FirstName,
                LastName = createdStudent.LastName,
                Email = createdStudent.Email,
                PhoneNumber = createdStudent.PhoneNumber,
                DateOfBirth = createdStudent.DateOfBirth
            };
        }

        //put -> dto to entity
        public async Task<bool> UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth
            };

            return await _repository.UpdateAsync(id, student);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}