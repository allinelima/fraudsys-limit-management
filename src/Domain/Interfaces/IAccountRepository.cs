namespace FraudSys.Domain.Interfaces;

public interface IAccountRepository
{
    // Busca uma conta pelo número
    Task<Account> GetByAccountNumberAsync(string accountNumber);
    
    // Adiciona uma nova conta
    Task AddAsync(Account account);
    
    // Atualiza uma conta existente
    Task UpdateAsync(Account account);
    
    // Remove uma conta
    Task DeleteAsync(string accountNumber);
}