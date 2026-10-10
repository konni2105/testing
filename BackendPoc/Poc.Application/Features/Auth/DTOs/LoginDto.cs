using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Application.Features.Auth.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
