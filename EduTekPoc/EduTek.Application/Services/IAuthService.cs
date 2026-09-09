using EduTek.Application.DTOs;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<UserDto?> ValidateCredentialsAsync(LoginDto dto);
}