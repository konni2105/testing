using System.ComponentModel.DataAnnotations;

namespace EduTek.Web.Models
{
    public class EditMarkViewModel
    {
        public int MarkId { get; set; }

        [Range(0, 100)]
        public decimal Score { get; set; }
    }
}