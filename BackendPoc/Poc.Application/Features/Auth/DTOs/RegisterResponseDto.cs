using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Application.Features.Auth.DTOs
{
    public class RegisterResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
