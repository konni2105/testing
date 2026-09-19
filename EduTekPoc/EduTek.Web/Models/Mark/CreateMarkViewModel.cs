using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class CreateMarkViewModel
    {
        [Required]
        public int ExamId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Range(0, 100)]
        public decimal Score { get; set; }
    }
}