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

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
    {
        var account = new Account(
            createAccountDto.Document,
            createAccountDto.Agency,
            createAccountDto.AccountNumber,
            createAccountDto.PixLimit
        );

        var createdAccount = await _accountRepository.AddAsync(account);
        if (createdAccount == null)
            throw new DomainException("Erro ao criar conta");

        return MapToDto(createdAccount);
    }

    public async Task<AccountDto> GetAccountAsync(string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");

        return MapToDto(account);
    }

    public async Task<AccountDto> UpdateLimitAsync(string accountNumber, UpdateAccountLimitDto updateLimitDto)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");

        account.UpdatePixLimit(updateLimitDto.NewLimit);
        var updatedAccount = await _accountRepository.UpdateAsync(account);
        if (updatedAccount == null)
            throw new DomainException("Erro ao atualizar conta");

        return MapToDto(updatedAccount);
    }

    public async Task DeleteAccountAsync(string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");

        await _accountRepository.DeleteAsync(accountNumber);
    }

    public async Task<TransactionResultDto> ProcessTransactionAsync(ProcessTransactionDto transactionDto)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(transactionDto.AccountNumber);
        if (account == null)
            throw new DomainException("Conta não encontrada");

        if (!account.CanProcessTransaction(transactionDto.Amount))
        {
            return new TransactionResultDto(
                success: false,
                message: "Limite insuficiente para a transação",
                newBalance: account.PixLimit
            );
        }

        account.DeductLimit(transactionDto.Amount);
        var updatedAccount = await _accountRepository.UpdateAsync(account);
        if (updatedAccount == null)
            throw new DomainException("Erro ao processar transação");

        return new TransactionResultDto(
            success: true,
            message: "Transação processada com sucesso",
            newBalance: updatedAccount.PixLimit
        );
    }

    private static AccountDto MapToDto(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        return new AccountDto(
            document: account.Document,
            agency: account.Agency,
            accountNumber: account.AccountNumber,
            pixLimit: account.PixLimit
        );
    }
}