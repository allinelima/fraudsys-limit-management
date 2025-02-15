using Xunit;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Exceptions;

namespace FraudSys.Tests.Domain;

public class AccountTests
{
    [Fact]
    public void CreateAccount_WithValidData_ShouldSucceed()
    {
        // Arrange
        var document = "12345678901";
        var agency = "0001";
        var accountNumber = "123456";
        var pixLimit = 1000m;

        // Act
        var account = new Account(document, agency, accountNumber, pixLimit);

        // Assert
        Assert.Equal(document, account.Document);
        Assert.Equal(agency, account.Agency);
        Assert.Equal(accountNumber, account.AccountNumber);
        Assert.Equal(pixLimit, account.PixLimit);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("1234567890a")]
    public void CreateAccount_WithInvalidDocument_ShouldThrowException(string invalidDocument)
    {
        // Arrange
        var agency = "0001";
        var accountNumber = "123456";
        var pixLimit = 1000m;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Account(invalidDocument, agency, accountNumber, pixLimit));
        Assert.Contains("documento", exception.Message.ToLower());
    }
}