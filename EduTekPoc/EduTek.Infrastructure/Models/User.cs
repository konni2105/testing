using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTek.Infrastructure.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // public string Role { get; set; } = "User"; // Admin, Teacher, Student, User

        [ForeignKey("RoleId")]
        public Role Role { get; set; } 

        public  int RoleId { get; set; } 

        public bool IsActive { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
