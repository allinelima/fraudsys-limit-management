using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Api.Controllers
{
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
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                var result = await _accountService.CreateAccountAsync(request);
                _logger.LogInformation("Conta criada com sucesso - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, result.AccountNumber);
                return Created($"api/accounts/{result.AccountNumber}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar conta - Correlation ID: {CorrelationId}", correlationId);
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDto>> GetAccount(string accountNumber)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                var account = await _accountService.GetAccountAsync(accountNumber);
                _logger.LogInformation("Conta encontrada - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, accountNumber);
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar conta - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, accountNumber);
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{accountNumber}/limit")]
        public async Task<ActionResult<AccountDto>> UpdateLimit(string accountNumber, [FromBody] UpdateAccountLimitDto request)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                var result = await _accountService.UpdateLimitAsync(accountNumber, request);
                _logger.LogInformation("Limite atualizado - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, accountNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar limite para conta - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, accountNumber);
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{accountNumber}")]
        public async Task<ActionResult> DeleteAccount(string accountNumber)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                await _accountService.DeleteAccountAsync(accountNumber);
                _logger.LogInformation("Conta excluída - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, accountNumber);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir conta - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, accountNumber);
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("transaction")]
        public async Task<ActionResult<TransactionResultDto>> ProcessTransaction([FromBody] ProcessTransactionDto request)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                var result = await _accountService.ProcessTransactionAsync(request);
                _logger.LogInformation("Transação processada - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, request.AccountNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar transação para conta - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, request.AccountNumber);
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
