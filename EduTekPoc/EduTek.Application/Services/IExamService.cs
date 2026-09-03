using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IExamService
    {
        Task<List<Exam>> GetAllAsync();

        Task<Exam?> GetByIdAsync(int id);

        Task<Exam> AddAsync(Exam exam);

        Task<bool> UpdateAsync(int id, Exam exam);

        Task<bool> DeleteAsync(int id);
    }
}