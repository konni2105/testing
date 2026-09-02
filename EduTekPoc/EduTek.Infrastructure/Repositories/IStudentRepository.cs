

using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();

        Task<Student?> GetByIdAsync(int id);

        Task<Student> AddAsync(Student student);

        Task<bool> UpdateAsync(int id, Student student);

        Task<bool> DeleteAsync(int id);
    }
}
