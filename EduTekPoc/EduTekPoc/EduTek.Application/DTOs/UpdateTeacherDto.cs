namespace EduTek.Application.DTOs
{
    public class UpdateTeacherDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Subject{ get; set; } = string.Empty;
    }
}