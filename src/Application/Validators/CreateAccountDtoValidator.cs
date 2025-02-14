using FluentValidation;
using FraudSys.Application.DTOs;

namespace FraudSys.Application.Validators;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountDtoValidator()
    {
        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Document is required")
            .Length(11).WithMessage("Document must have 11 digits")
            .Matches("^[0-9]+$").WithMessage("Document must contain only numbers");

        RuleFor(x => x.Agency)
            .NotEmpty().WithMessage("Agency is required")
            .Matches("^[0-9]+$").WithMessage("Agency must contain only numbers");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .Matches("^[0-9]+$").WithMessage("Account number must contain only numbers");

        RuleFor(x => x.PixLimit)
            .GreaterThanOrEqualTo(0).WithMessage("PIX limit cannot be negative");
    }
}