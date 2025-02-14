using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountDto request)
    {
        try
        {
            var result = await _accountService.CreateAccountAsync(request);
            return Created($"api/accounts/{result.AccountNumber}", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar conta");
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("{accountNumber}")]
    public async Task<ActionResult<AccountDto>> GetAccount(string accountNumber)
    {
        try
        {
            var account = await _accountService.GetAccountAsync(accountNumber);
            return Ok(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar conta {AccountNumber}", accountNumber);
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{accountNumber}/limit")]
    public async Task<ActionResult<AccountDto>> UpdateLimit(string accountNumber, [FromBody] UpdateAccountLimitDto request)
    {
        try
        {
            var result = await _accountService.UpdateLimitAsync(accountNumber, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar limite para conta {AccountNumber}", accountNumber);
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{accountNumber}")]
    public async Task<ActionResult> DeleteAccount(string accountNumber)
    {
        try
        {
            await _accountService.DeleteAccountAsync(accountNumber);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir conta {AccountNumber}", accountNumber);
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPost("transaction")]
    public async Task<ActionResult<TransactionResultDto>> ProcessTransaction([FromBody] ProcessTransactionDto request)
    {
        try
        {
            var result = await _accountService.ProcessTransactionAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar transação para conta {AccountNumber}", request.AccountNumber);
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}