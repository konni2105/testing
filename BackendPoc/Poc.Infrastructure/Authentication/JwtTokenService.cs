using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Poc.Application.Features.Auth.DTOs;
using Poc.Application.Features.Auth.Interfaces;
using Poc.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Poc.Infrastructure.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponseDto GenerateToken(User user)
        {
            string issuer =
                _configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException(
                    "JWT issuer is missing.");

            string audience =
                _configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException(
                    "JWT audience is missing.");

            string secretKey =
                _configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException(
                    "JWT secret key is missing.");

            if (Encoding.UTF8.GetByteCount(secretKey) < 32)
            {
                throw new InvalidOperationException(
                    "JWT secret key must contain at least 32 bytes.");
            }

            if (!int.TryParse(
                _configuration["JwtSettings:ExpiryMinutes"],
                out int expiryMinutes) || expiryMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "JWT expiry must be greater than zero.");
            }

            DateTime expiresAtUtc =
                DateTime.UtcNow.AddMinutes(expiryMinutes);

            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.UniqueName,
                    user.Username),

                new Claim(
                    ClaimTypes.Role,
                    user.Role.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            string tokenString =
                new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                AccessToken = tokenString,
                ExpiresAtUtc = TimeZoneInfo.ConvertTimeFromUtc(
                    expiresAtUtc,
                    TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
            };
        }
    }
}