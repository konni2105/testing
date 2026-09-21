namespace EduTek.Web.Models
{

    public class DashboardViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public int SubjectCount { get; set; }
        public int ClassCount { get; set; }

        public int PendingRegistrationCount { get; set; }
    }
}