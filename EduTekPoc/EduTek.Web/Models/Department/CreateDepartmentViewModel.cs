using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class CreateDepartmentViewModel
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}