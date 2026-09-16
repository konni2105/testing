

namespace EduTek.Application.DTOs
{
    public class BulkApprovalDto
    {
        public List<int> UserIds { get; set; } = new();
        public bool IsApproved { get; set; }
    }
}