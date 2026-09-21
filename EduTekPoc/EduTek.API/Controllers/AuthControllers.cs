using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Data;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

 

        public AuthController(IAuthService authService, IConfiguration configuration )
        {
            _authService = authService;
            _configuration = configuration;
           
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

        // POST: /api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.ValidateCredentialsAsync(dto);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            //var tokenResponse = GenerateJwtToken(user.Username, user.Role);
            //return Ok(tokenResponse);
            var tokenResponse = await _authService.LoginAsync(dto);

            return Ok(tokenResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
    [FromBody] RefreshTokenRequestDto dto)
        {
            var response =
                await _authService.RefreshTokenAsync(dto.RefreshToken);

            if (response == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });
            }

            return Ok(response);
        }






    }
}