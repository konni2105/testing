 
using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> AddAsync(Student student)
        {
            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<bool> UpdateAsync(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return false;
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.DateOfBirth = student.DateOfBirth;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}