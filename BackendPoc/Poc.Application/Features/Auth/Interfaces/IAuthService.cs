
using Poc.Application.Features.Auth.DTOs;

namespace Poc.Application.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}