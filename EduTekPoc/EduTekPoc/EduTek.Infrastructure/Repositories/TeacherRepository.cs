
 
using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _context.Teachers.FindAsync(id);
        }

        public async Task<Teacher> AddAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);

            await _context.SaveChangesAsync();

            return teacher;
        }

        public async Task<bool> UpdateAsync(int id, Teacher teacher)
        {
            var existingTeacher = await _context.Teachers.FindAsync(id);

            if (existingTeacher == null)
            {
                return false;
            }

            existingTeacher.FirstName = teacher.FirstName;
            existingTeacher.LastName = teacher.LastName;
            existingTeacher.Email = teacher.Email;
            existingTeacher.PhoneNumber = teacher.PhoneNumber;
            existingTeacher.Subject = teacher.Subject;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return false;
            }

            _context.Teachers.Remove(teacher);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}