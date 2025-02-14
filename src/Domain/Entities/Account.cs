using FraudSys.Domain.Exceptions;

namespace FraudSys.Domain.Entities;

public class Account
{
    // Propriedades com setters privados para garantir encapsulamento
    public string Document { get; private set; }  // CPF
    public string Agency { get; private set; }
    public string AccountNumber { get; private set; }
    public decimal PixLimit { get; private set; }

    // Construtor com validações
    public Account(string document, string agency, string accountNumber, decimal pixLimit)
    {
        // Validamos todos os campos antes de criar a conta
        ValidateDocument(document);
        ValidateAgency(agency);
        ValidateAccountNumber(accountNumber);
        ValidatePixLimit(pixLimit);

        Document = document;
        Agency = agency;
        AccountNumber = accountNumber;
        PixLimit = pixLimit;
    }

    // Método que verifica se uma transação pode ser processada
    public bool CanProcessTransaction(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("O valor da transação deve ser maior que zero");
            
        return PixLimit >= amount;
    }

    // Método para atualizar o limite PIX
    public void UpdateLimit(decimal newLimit)
    {
        ValidatePixLimit(newLimit);
        PixLimit = newLimit;
    }

    // Método para deduzir um valor do limite
    public void DeductLimit(decimal amount)
    {
        if (!CanProcessTransaction(amount))
            throw new DomainException("Limite insuficiente para a transação");
            
        PixLimit -= amount;
    }

    // Validações privadas
    private void ValidateDocument(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            throw new DomainException("O documento não pode estar vazio");
            
        if (document.Length != 11)
            throw new DomainException("O documento deve ter 11 dígitos");
            
        if (!document.All(char.IsDigit))
            throw new DomainException("O documento deve conter apenas números");
    }

    private void ValidateAgency(string agency)
    {
        if (string.IsNullOrWhiteSpace(agency))
            throw new DomainException("A agência não pode estar vazia");
            
        if (!agency.All(char.IsDigit))
            throw new DomainException("A agência deve conter apenas números");
    }

    private void ValidateAccountNumber(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new DomainException("O número da conta não pode estar vazio");
            
        if (!accountNumber.All(char.IsDigit))
            throw new DomainException("O número da conta deve conter apenas números");
    }

    private void ValidatePixLimit(decimal pixLimit)
    {
        if (pixLimit < 0)
            throw new DomainException("O limite PIX não pode ser negativo");
    }
}