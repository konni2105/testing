using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            // Simple credential validation
            if (username != "admin" || password != "1234")
            {
                return Unauthorized("Invalid username or password");
            }

            // Claims = information stored inside JWT
            var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

            // Same secret key we configured in Program.cs
            var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("EduTekSuperSecretKey1234567890ABC"));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Create JWT
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new
            {
                token = tokenString
            });
        }
    }
}