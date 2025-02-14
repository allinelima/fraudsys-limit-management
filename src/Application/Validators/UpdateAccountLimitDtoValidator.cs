using FluentValidation;
using FraudSys.Application.DTOs;

namespace FraudSys.Application.Validators;

public class UpdateAccountLimitDtoValidator : AbstractValidator<UpdateAccountLimitDto>
{
    public UpdateAccountLimitDtoValidator()
    {
        RuleFor(x => x.NewPixLimit)
            .GreaterThanOrEqualTo(0).WithMessage("PIX limit cannot be negative");
    }
}
