using FluentValidation;
using FraudSys.Application.DTOs;

namespace FraudSys.Application.Validators;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountDtoValidator()
    {
        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("O documento é obrigatório")
            .Length(11).WithMessage("O documento deve ter 11 dígitos")
            .Matches("^[0-9]+$").WithMessage("O documento deve conter apenas números");

        RuleFor(x => x.Agency)
            .NotEmpty().WithMessage("A agência é obrigatória")
            .Matches("^[0-9]+$").WithMessage("A agência deve conter apenas números");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("O número da conta é obrigatório")
            .Matches("^[0-9]+$").WithMessage("O número da conta deve conter apenas números");

        RuleFor(x => x.PixLimit)
            .GreaterThanOrEqualTo(0).WithMessage("O limite PIX não pode ser negativo");
    }
}