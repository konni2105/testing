using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PersonInfo.Models
{
    [Index(nameof(Email),IsUnique=true)]         //Added the unique key for the Email and UserName. need to handle duplicate. 
    [Index(nameof(UserName), IsUnique = true)]

    public class UserInfoAccount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string Password { get; set; }

    }
}
