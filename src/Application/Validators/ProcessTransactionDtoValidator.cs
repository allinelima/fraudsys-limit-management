using FluentValidation;
using FraudSys.Application.DTOs;

namespace FraudSys.Application.Validators;

public class ProcessTransactionDtoValidator : AbstractValidator<ProcessTransactionDto>
{
    public ProcessTransactionDtoValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .Matches("^[0-9]+$").WithMessage("Account number must contain only numbers");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Transaction amount must be greater than zero");
    }
}