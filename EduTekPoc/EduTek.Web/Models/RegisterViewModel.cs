using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }
    }
}