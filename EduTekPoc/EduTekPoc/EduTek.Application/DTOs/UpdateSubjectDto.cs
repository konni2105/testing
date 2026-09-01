using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduTek.Application.DTOs
{
   public class UpdateSubjectDto
    {
        [Required]
        [StringLength(100)]
        public string SubjectName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
