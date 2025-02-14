using FraudSys.Application.DTOs;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using FraudSys.Domain.Exceptions;

namespace FraudSys.Application.Services;


public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto)
    {
        var account = new Account(dto.Document, dto.Agency, dto.AccountNumber, dto.PixLimit);
        await _accountRepository.AddAsync(account);
        return MapToDto(account);
    }

    public async Task<AccountDto> GetAccountAsync(string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");
            
        return MapToDto(account);
    }

    public async Task<AccountDto> UpdateLimitAsync(string accountNumber, UpdateAccountLimitDto dto)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");

        account.UpdateLimit(dto.NewPixLimit);
        await _accountRepository.UpdateAsync(account);
        
        return MapToDto(account);
    }

    public async Task DeleteAccountAsync(string accountNumber)
    {
        await _accountRepository.DeleteAsync(accountNumber);
    }

    public async Task<TransactionResultDto> ProcessTransactionAsync(ProcessTransactionDto dto)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(dto.AccountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");

        if (!account.CanProcessTransaction(dto.Amount))
            return new TransactionResultDto { IsApproved = false, Message = "Limite insuficiente" };

        account.DeductLimit(dto.Amount);
        await _accountRepository.UpdateAsync(account);

        return new TransactionResultDto { IsApproved = true };
    }

    private static AccountDto MapToDto(Account account)
    {
        return new AccountDto
        {
            Document = account.Document,
            Agency = account.Agency,
            AccountNumber = account.AccountNumber,
            PixLimit = account.PixLimit
        };
    }
}