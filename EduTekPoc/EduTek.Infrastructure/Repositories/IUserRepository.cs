using System.Threading.Tasks;
using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<List<User>> GetPendingUsersAsync();

        Task<bool> UpdateAsync(User user);


    }
}
