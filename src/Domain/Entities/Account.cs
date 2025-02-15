using FraudSys.Domain.Exceptions;

namespace FraudSys.Domain.Entities;

public class Account
{
    public string Document { get; set; } = string.Empty;
    public string Agency { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal PixLimit { get; private set; }

    public Account(string document, string agency, string accountNumber, decimal pixLimit)
    {
        ValidateDocument(document);
        ValidateAgency(agency);
        ValidateAccountNumber(accountNumber);
        ValidatePixLimit(pixLimit);

        Document = document;
        Agency = agency;
        AccountNumber = accountNumber;
        PixLimit = pixLimit;
    }

    public bool CanProcessTransaction(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("O valor da transação deve ser maior que zero");
            
        return PixLimit >= amount;
    }

    public void UpdatePixLimit(decimal newLimit)
    {
        ValidatePixLimit(newLimit);
        PixLimit = newLimit;
    }

    public void DeductLimit(decimal amount)
    {
        if (!CanProcessTransaction(amount))
            throw new DomainException("Limite insuficiente para a transação");
            
        PixLimit -= amount;
    }

    private void ValidateDocument(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            throw new DomainException("O documento é obrigatório");

        if (document.Length != 11)
            throw new DomainException("O documento deve ter 11 dígitos");

        if (!document.All(char.IsDigit))
            throw new DomainException("O documento deve conter apenas números");
    }

    private void ValidateAgency(string agency)
    {
        if (string.IsNullOrWhiteSpace(agency))
            throw new DomainException("A agência é obrigatória");

        if (!agency.All(char.IsDigit))
            throw new DomainException("A agência deve conter apenas números");
    }

    private void ValidateAccountNumber(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new DomainException("O número da conta é obrigatório");

        if (!accountNumber.All(char.IsDigit))
            throw new DomainException("O número da conta deve conter apenas números");
    }

    private void ValidatePixLimit(decimal pixLimit)
    {
        if (pixLimit < 0)
            throw new DomainException("O limite PIX não pode ser negativo");
    }
}