using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IMarkService
    {
        Task<List<Mark>> GetAllAsync();

        Task<Mark?> GetByIdAsync(int id);

        Task<Mark> AddAsync(Mark mark);

        Task<bool> UpdateAsync(int id, Mark mark);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int examId, int studentId);
    }
}