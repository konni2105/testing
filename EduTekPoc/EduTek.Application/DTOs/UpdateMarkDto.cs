using System.ComponentModel.DataAnnotations;

namespace EduTek.Application.DTOs
{
    public class UpdateMarkDto
    {
        [Range(0, 100)]
        public decimal Score { get; set; }
    }
}