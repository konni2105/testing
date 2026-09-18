using System.ComponentModel.DataAnnotations;
namespace EduTek.Web.Models
{
    public class EditStudentViewModel
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int ClassId { get; set; }
    }
}