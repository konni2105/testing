using Poc.Application.Features.Auth.DTOs;
using Poc.Domain.Entities;

namespace Poc.Application.Features.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        AuthResponseDto GenerateToken(User user);
    }
}