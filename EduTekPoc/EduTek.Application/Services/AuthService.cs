using System;
using System.Threading.Tasks;
using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasherService passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.ExistsByUsernameAsync(dto.Username))
            {
                throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");
            }

            var passwordHash = _passwordHasher.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);

            return MapToDto(createdUser);
        }

        public async Task<UserDto?> ValidateCredentialsAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            var isValidPassword = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                return null;
            }

            return MapToDto(user);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
