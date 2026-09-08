using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();

        Task<Student?> GetByIdAsync(int id);

        Task<Student> AddAsync(Student student);

        Task<bool> UpdateAsync(
            int id,
            Student student);

        Task<bool> DeleteAsync(int id);

        Task<bool> EmailExistsAsync(
            string email);

        Task<bool> EmailExistsForOtherStudentAsync(
            string email,
            int studentId);

        Task<bool> HasAttendanceAsync(
            int studentId);
    }
}