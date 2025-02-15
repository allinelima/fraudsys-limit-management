using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(IAccountService accountService, ILogger<TransactionController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost("pix")]
    public async Task<ActionResult<TransactionResultDto>> ProcessPixTransaction([FromBody] ProcessTransactionDto request)
    {
        try
        {
            var result = await _accountService.ProcessTransactionAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar transação PIX para conta {AccountNumber}", request.AccountNumber);
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}