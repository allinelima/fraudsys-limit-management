using FraudSys.Api.Controllers;
using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FraudSys.Tests.Controllers
{
    public class TransactionControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ILogger<TransactionController>> _loggerMock;
        private readonly TransactionController _controller;

        public TransactionControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _loggerMock = new Mock<ILogger<TransactionController>>();
            _controller = new TransactionController(_accountServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ProcessPixTransaction_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new ProcessTransactionDto("123456", 100.00m);
            var expectedResult = new TransactionResultDto(true, "Transação aprovada", 900.00m);
            
            _accountServiceMock
                .Setup(x => x.ProcessTransactionAsync(It.IsAny<ProcessTransactionDto>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ProcessPixTransaction(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TransactionResultDto>(okResult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal(expectedResult.Message, returnValue.Message);
            Assert.Equal(expectedResult.NewBalance, returnValue.NewBalance);

            // Verificar se o Correlation ID foi logado
            _loggerMock.Verify(log => log.LogInformation(It.Is<string>(s => s.Contains("Correlation ID:"))), Times.Once);
        }

        [Fact]
        public async Task ProcessPixTransaction_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProcessTransactionDto("123456", -100.00m);
            _accountServiceMock
                .Setup(x => x.ProcessTransactionAsync(It.IsAny<ProcessTransactionDto>()))
                .ThrowsAsync(new ArgumentException("Valor inválido"));

            // Act
            var result = await _controller.ProcessPixTransaction(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);

            // Verificar se o Correlation ID foi logado
            _loggerMock.Verify(log => log.LogError(It.Is<string>(s => s.Contains("Correlation ID:"))), Times.Once);
        }

        [Fact]
        public async Task ProcessPixTransaction_InsufficientLimit_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProcessTransactionDto("123456", 5000.00m);
            _accountServiceMock
                .Setup(x => x.ProcessTransactionAsync(It.IsAny<ProcessTransactionDto>()))
                .ThrowsAsync(new InvalidOperationException("Limite insuficiente"));

            // Act
            var result = await _controller.ProcessPixTransaction(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);

            // Verificar se o Correlation ID foi logado
            _loggerMock.Verify(log => log.LogError(It.Is<string>(s => s.Contains("Correlation ID:"))), Times.Once);
        }
    }
}
