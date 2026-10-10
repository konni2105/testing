
namespace Poc.Application.Common.DTOs;

public class ApiResponseDto<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public static ApiResponseDto<T> CreateSuccess(
        string message,
        T? data)
    {
        return new ApiResponseDto<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
}
