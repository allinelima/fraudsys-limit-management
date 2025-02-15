using FraudSys.Domain.Entities;

namespace FraudSys.Domain.Interfaces;

public interface IAccountRepository
{
    // Busca uma conta pelo número
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    
    // Adiciona uma nova conta
    Task<Account?> AddAsync(Account account);
    
    // Atualiza uma conta existente
    Task<Account?> UpdateAsync(Account account);
    
    // Remove uma conta
    Task DeleteAsync(string accountNumber);
}