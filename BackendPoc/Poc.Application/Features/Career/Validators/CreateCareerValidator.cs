using FluentValidation;
using Poc.Application.Features.Career.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Application.Features.Career.Validators
{
    public class CreateCareerValidator : AbstractValidator<CreateCareerDto>
    {
        public CreateCareerValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.MinimumSalary)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaximumSalary)
                .GreaterThanOrEqualTo(x => x.MinimumSalary);

            RuleFor(x => x.RequiredExperience)
                .GreaterThanOrEqualTo(0);
        }
    }
}
