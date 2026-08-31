using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonInfo.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        [EmailAddress(ErrorMessage ="Please Enter valid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength =5, ErrorMessage = "Max 20 or min 5 characters allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Password not matching")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
