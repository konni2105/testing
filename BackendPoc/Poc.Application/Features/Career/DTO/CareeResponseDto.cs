using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Application.Features.Career.DTO
{
    public class CareerResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal MinimumSalary { get; set; }

        public decimal MaximumSalary { get; set; }

        public int RequiredExperience { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
