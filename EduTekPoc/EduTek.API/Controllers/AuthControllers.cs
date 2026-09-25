using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var user = await _authService.RegisterAsync(dto);

                return Ok(new
                {
                    message = user.IsActive
                        ? "Admin registered successfully. You can login."
                        : "Registered successfully. Wait for admin approval.",
                    userId = user.UserId
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var tokenResponse = await _authService.LoginAsync(dto);

            if (tokenResponse == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(tokenResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequestDto dto)
        {
            var response = await _authService.RefreshTokenAsync(dto.RefreshToken);

            if (response == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });
            }

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody] RefreshTokenRequestDto dto)
        {
            var revoked = await _authService.RevokeRefreshTokenAsync(dto.RefreshToken);

            if (!revoked)
            {
                return Unauthorized(new
                {
                    message = "Invalid refresh token."
                });
            }

            return Ok(new
            {
                message = "Logged out successfully. Refresh token has been revoked."
            });
        }
    }
}
