using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes.FindAsync(id);
        }

        public async Task<Class> AddAsync(Class classEntity)
        {
            _context.Classes.Add(classEntity);

            await _context.SaveChangesAsync();

            return classEntity;
        }

        public async Task<bool> UpdateAsync(int id, Class classEntity)
        {
            var existingClass = await _context.Classes.FindAsync(id);

            if (existingClass == null)
            {
                return false;
            }

            existingClass.ClassName = classEntity.ClassName;
            existingClass.Description = classEntity.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingClass = await _context.Classes.FindAsync(id);

            if (existingClass == null)
            {
                return false;
            }

            _context.Classes.Remove(existingClass);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}