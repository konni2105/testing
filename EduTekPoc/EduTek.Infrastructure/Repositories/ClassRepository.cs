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

        public async Task<bool> NameExistsAsync(string className)
        {
            return await _context.Classes
                .AnyAsync(c => c.ClassName == className);
        }

        //for update
        public async Task<bool> NameExistsForOtherClassAsync(
            string className,
            int classId)
                {
            return await _context.Classes
                .AnyAsync(c =>
                    c.ClassName == className &&
                    c.ClassId != classId);
        }

        //student relationship
        public async Task<bool> HasStudentsAsync(int classId)
        {
            return await _context.Students
                .AnyAsync(s => s.ClassId == classId);
        }

        //ClassSubject relationship
        public async Task<bool> HasSubjectAssignmentsAsync(int classId)
        {
            return await _context.ClassSubjects
                .AnyAsync(x => x.ClassId == classId);
        }

        // TeacherSubjectClass relationship
        public async Task<bool> HasTeacherAssignmentsAsync(int classId)
        {
            return await _context.TeacherSubjectClasses
                .AnyAsync(x => x.ClassId == classId);
        }
        //exam relationship
        public async Task<bool> HasExamsAsync(int classId)
        {
            return await _context.Exams
                .AnyAsync(x => x.ClassId == classId);
        }
    }
}