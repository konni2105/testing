
using Microsoft.AspNetCore.Mvc;
using Poc.Application.Common.DTOs;
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

            return Ok(ApiResponseDto<RegisterResponseDto>.CreateSuccess(
                "User registered successfully. Please log in.",
                result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(ApiResponseDto<AuthResponseDto>.CreateSuccess(
                "Login successful.",
                result));
        }
    }
}