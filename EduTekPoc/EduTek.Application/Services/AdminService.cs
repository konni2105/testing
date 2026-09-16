using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<PendingUserDto>> GetPendingRegistrationsAsync()
        {
            var users = await _userRepository.GetPendingUsersAsync();

            return users.Select(u => new PendingUserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role.Name
            }).ToList();
        }

        public async Task<bool> ApproveRegistrationAsync(ApprovalDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                return false;
            }

            user.IsActive = dto.IsApproved;

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> BulkApproveRegistrationsAsync(BulkApprovalDto dto)
        {
            foreach (var userId in dto.UserIds)
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user != null)
                {
                    user.IsActive = dto.IsApproved;
                    await _userRepository.UpdateAsync(user);
                }
            }

            return true;
        }
    }
}