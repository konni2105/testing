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
        // Temporary users for POC/testing
        private static readonly List<(string Username, string Password, string Role)> Users =
            new()
            {
                ("admin", "1234", "Admin"),
                ("teacher1", "1234", "Teacher"),
                ("student1", "1234", "Student"),
                ("parent1", "1234", "Parent")
            };

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password);

            if (user == default)
            {
                return Unauthorized("Invalid username or password");
            }

            // Claims = information stored inside JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    "EduTekSuperSecretKey1234567890ABC"));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

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