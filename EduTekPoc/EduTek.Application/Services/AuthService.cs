using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EduTek.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(
                 IUserRepository userRepository,
                 IPasswordHasherService passwordHasher,
                 IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            if (dto.RoleId != 2 && dto.RoleId != 3)
            {
                throw new InvalidOperationException(
                    "Public registration is limited to Teacher or Student roles.");
            }

            if (await _userRepository.ExistsByUsernameAsync(dto.Username))
            {
                throw new InvalidOperationException(
                    $"Username '{dto.Username}' is already taken.");
            }

            var passwordHash =
                _passwordHasher.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                RoleId = dto.RoleId,
                IsActive = dto.RoleId == 1,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser =
                await _userRepository.AddAsync(user);

            return MapToDto(createdUser);
        }

        public async Task<UserDto?> ValidateCredentialsAsync(
            LoginDto dto)
        {
            var user =
                await _userRepository.GetByUsernameAsync(dto.Username);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            var isValidPassword =
                _passwordHasher.VerifyPassword(
                    dto.Password,
                    user.PasswordHash);

            if (!isValidPassword)
            {
                return null;
            }

            return MapToDto(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            var isValidPassword = _passwordHasher.VerifyPassword(
                dto.Password,
                user.PasswordHash);

            if (!isValidPassword)
            {
                return null;
            }

            var role = user.Role?.Name ?? string.Empty;

            var tokenResponse = GenerateJwtToken(
                user.Username,
                role);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);

            tokenResponse.RefreshToken = user.RefreshToken;

            return tokenResponse;
        }
       
       
        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            if (user.RefreshTokenExpiryTime == null ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var role = user.Role?.Name ?? string.Empty;

            var tokenResponse = GenerateJwtToken(
                user.Username,
                role);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);

            tokenResponse.RefreshToken = user.RefreshToken;

            return tokenResponse;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                return false;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            return await _userRepository.UpdateAsync(user);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        private AuthResponseDto GenerateJwtToken(string username, string role)
        {
            var jwtSecret = _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

            var expirationMinutes =
                int.Parse(_configuration["Jwt:DurationInMinutes"] ?? "30");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role)
    };

            var expiration =
                DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            var tokenString =
                new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                Username = username,
                Role = role,
                Expiration = expiration
            };
        }


        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role?.Name ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}