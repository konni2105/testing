using System;
using System.Collections.Generic;
using System.Text;

namespace EduTek.Application.DTOs
{
   public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
