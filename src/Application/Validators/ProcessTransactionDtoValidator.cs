using FluentValidation;
using FraudSys.Application.DTOs;

namespace FraudSys.Application.Validators;

public class ProcessTransactionDtoValidator : AbstractValidator<ProcessTransactionDto>
{
    public ProcessTransactionDtoValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("O número da conta é obrigatório")
            .Matches("^[0-9]+$").WithMessage("O número da conta deve conter apenas números");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero");
    }
}