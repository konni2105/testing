using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Poc.Application.Common.Interfaces;
using Poc.Application.Features.Auth.DTOs;
using Poc.Application.Features.Auth.Interfaces;
using Poc.Domain.Entities;
using Poc.Domain.Enum;

namespace Poc.Application.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IAppDbContext context,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Email)||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException(
                    "Username and password are required.");
            }

            string username = dto.Username.Trim();

            bool exists = await _context.Users
                .AnyAsync(u => u.Username == username);

            if (exists)
            {
                throw new InvalidOperationException(
                    "Username already exists.");
            }

            var user = new User
            {
                Username = username,
                Email = dto.Email.Trim(),
                Role = UserRole.User,
                CreatedDate = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _jwtTokenService.GenerateToken(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException(
                    "Username and password are required.");
            }

            string username = dto.Username.Trim();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid username or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    "Invalid username or password.");
            }

            return _jwtTokenService.GenerateToken(user);
        }
    }
}