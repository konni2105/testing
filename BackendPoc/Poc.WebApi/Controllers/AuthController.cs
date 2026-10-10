using Microsoft.AspNetCore.Mvc;
using Poc.Application.Common.Responses;
using Poc.Application.Features.Auth.DTOs;
using Poc.Application.Features.Auth.Interfaces;

namespace Poc.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var result = await _authService.RegisterAsync(dto);

            return Ok(new ApiResponse<AuthResponseDto>(
                true, "User registered successfully.", result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(new ApiResponse<AuthResponseDto>(
                true, "Login successful.", result));
        }
    }
}