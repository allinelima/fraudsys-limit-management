using Xunit;
using Moq;
using FraudSys.Domain.Interfaces;
using FraudSys.Application.Services;
using FraudSys.Domain.Entities;
using FraudSys.Application.DTOs;

namespace FraudSys.Tests.Application.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _repositoryMock;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _repositoryMock = new Mock<IAccountRepository>();
        _service = new AccountService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAccount_WithValidData_ShouldSucceed()
    {
        // Arrange
        var createDto = new CreateAccountDto
        {
            Document = "12345678901",
            Agency = "0001",
            AccountNumber = "123456",
            PixLimit = 1000m
        };

        // Act
        var result = await _service.CreateAccountAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Document, result.Document);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Account>()), Times.Once);
    }
}