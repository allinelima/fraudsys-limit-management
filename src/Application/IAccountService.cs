namespace FraudSys.Application.Services;

using FraudSys.Application.DTOs;

public interface IAccountService
{
    Task<AccountDto> CreateAccountAsync(CreateAccountDto dto);
    Task<AccountDto> GetAccountAsync(string accountNumber);
    Task<AccountDto> UpdateLimitAsync(string accountNumber, UpdateAccountLimitDto dto);
    Task DeleteAccountAsync(string accountNumber);
    Task<TransactionResultDto> ProcessTransactionAsync(ProcessTransactionDto dto);
}
