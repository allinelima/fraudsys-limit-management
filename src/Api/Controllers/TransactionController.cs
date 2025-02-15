using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Api.Controllers
{
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
            // Obter o Correlation ID do contexto HTTP, se não encontrado, gera um novo ID
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                // Log de início da transação
                _logger.LogInformation("Iniciando transação PIX - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, request.AccountNumber);

                // Processa a transação
                var result = await _accountService.ProcessTransactionAsync(request);

                // Log de sucesso
                _logger.LogInformation("Transação processada com sucesso - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, request.AccountNumber);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log de erro
                _logger.LogError(ex, "Erro ao processar transação PIX - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, request.AccountNumber);

                // Retorno de erro para o cliente
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
