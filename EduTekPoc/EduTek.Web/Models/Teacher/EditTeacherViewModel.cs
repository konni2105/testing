using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class EditTeacherViewModel
    {
        public int TeacherId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(
            10,
            MinimumLength = 10,
            ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}