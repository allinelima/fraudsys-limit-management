using FluentValidation;
using FraudSys.Application.DTOs;

namespace FraudSys.Application.Validators;

public class UpdateAccountLimitDtoValidator : AbstractValidator<UpdateAccountLimitDto>
{
    public UpdateAccountLimitDtoValidator()
    {
        RuleFor(x => x.NewLimit)
            .GreaterThanOrEqualTo(0).WithMessage("O limite PIX não pode ser negativo");
    }
}
