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

        // GET ALL
        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students
                .ToListAsync();
        }

        // GET BY ID
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FindAsync(id);
        }

        // CREATE
        public async Task<Student> AddAsync(
            Student student)
        {
            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return student;
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id,
            Student student)
        {
            var existingStudent =
                await _context.Students.FindAsync(id);

            if (existingStudent == null)
                return false;

            existingStudent.FirstName =
                student.FirstName;

            existingStudent.LastName =
                student.LastName;

            existingStudent.Email =
                student.Email;

            existingStudent.PhoneNumber =
                student.PhoneNumber;

            existingStudent.DateOfBirth =
                student.DateOfBirth;

            existingStudent.ClassId =
                student.ClassId;

            await _context.SaveChangesAsync();

            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var student =
                await _context.Students.FindAsync(id);

            if (student == null)
                return false;

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return true;
        }

        // CHECK EMAIL
        public async Task<bool> EmailExistsAsync(
            string email)
        {
            return await _context.Students
                .AnyAsync(s => s.Email == email);
        }

        // CHECK EMAIL FOR OTHER STUDENT
        public async Task<bool> EmailExistsForOtherStudentAsync(
            string email,
            int studentId)
        {
            return await _context.Students
                .AnyAsync(s =>
                    s.Email == email &&
                    s.StudentId != studentId);
        }

        // CHECK ATTENDANCE
        public async Task<bool> HasAttendanceAsync(
            int studentId)
        {
            return await _context.Attendances
                .AnyAsync(a =>
                    a.StudentId == studentId);
        }
    }
}