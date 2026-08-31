using System;

namespace EduTek.Infrastructure.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; } = string.Empty;//instead of leaving null gives initial empty valu

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
    }
}
