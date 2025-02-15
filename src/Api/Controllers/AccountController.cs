using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FraudSys.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        // Método para exibir a criação de uma conta
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Método para processar a criação de uma conta
        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDto request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.CreateAccountAsync(request);
                    _logger.LogInformation("Conta criada com sucesso, Conta: {AccountNumber}", result.AccountNumber);
                    return RedirectToAction("Details", new { accountNumber = result.AccountNumber });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar conta");
                    ModelState.AddModelError("", "Erro ao criar a conta.");
                }
            }
            return View(request);
        }

        // Método para exibir uma conta específica
        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> Details(string accountNumber)
        {
            var account = await _accountService.GetAccountAsync(accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // Método para atualizar o limite da conta
        [HttpGet("{accountNumber}/limit")]
        public IActionResult UpdateLimit(string accountNumber)
        {
            var model = new UpdateAccountLimitDto { AccountNumber = accountNumber };
            return View(model);
        }

        [HttpPost("{accountNumber}/limit")]
        public async Task<IActionResult> UpdateLimit(string accountNumber, UpdateAccountLimitDto request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.UpdateLimitAsync(accountNumber, request);
                    return RedirectToAction("Details", new { accountNumber = result.AccountNumber });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar limite para conta: {AccountNumber}", accountNumber);
                    ModelState.AddModelError("", "Erro ao atualizar o limite.");
                }
            }
            return View(request);
        }

        // Método para excluir uma conta
        [HttpDelete("{accountNumber}")]
        public async Task<IActionResult> DeleteAccount(string accountNumber)
        {
            try
            {
                await _accountService.DeleteAccountAsync(accountNumber);
                _logger.LogInformation("Conta excluída com sucesso - Conta: {AccountNumber}", accountNumber);
                return RedirectToAction("Index"); // ou outra página de listagem
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir conta: {AccountNumber}", accountNumber);
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // Método para processar transações
        [HttpPost("transaction")]
        public async Task<IActionResult> ProcessTransaction([FromBody] ProcessTransactionDto request)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            try
            {
                var result = await _accountService.ProcessTransactionAsync(request);
                _logger.LogInformation("Transação processada com sucesso - Correlation ID: {CorrelationId}, Conta: {AccountNumber}", correlationId, request.AccountNumber);
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
