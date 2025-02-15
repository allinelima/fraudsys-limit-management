using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using FraudSys.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace FraudSys.Infrastructure.Data.DynamoDb;

public class AccountRepository : IAccountRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName;

    public AccountRepository(IAmazonDynamoDB dynamoDb, IConfiguration configuration)
    {
        _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        _tableName = configuration["DynamoDB:TableName"] ?? throw new ArgumentNullException("DynamoDB:TableName não configurado");
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber))
            throw new ArgumentNullException(nameof(accountNumber));

        try
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "AccountNumber", new AttributeValue { S = accountNumber } }
                }
            };

            var response = await _dynamoDb.GetItemAsync(request);
            
            if (response.Item == null || !response.Item.Any())
                return null;

            return MapToAccount(response.Item);
        }
        catch (AmazonDynamoDBException ex)
        {
            throw new DomainException($"Erro ao buscar conta: {ex.Message}", ex);
        }
    }

    public async Task<Account?> AddAsync(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        try
        {
            var item = MapFromAccount(account);

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = item,
                ConditionExpression = "attribute_not_exists(AccountNumber)"
            };

            await _dynamoDb.PutItemAsync(request);
            
            return account;
        }
        catch (ConditionalCheckFailedException)
        {
            throw new DomainException($"Conta com número {account.AccountNumber} já existe");
        }
        catch (AmazonDynamoDBException ex)
        {
            throw new DomainException($"Erro ao adicionar conta: {ex.Message}", ex);
        }
    }

    public async Task<Account?> UpdateAsync(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        try
        {
            var item = MapFromAccount(account);

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = item,
                ConditionExpression = "attribute_exists(AccountNumber)"
            };

            await _dynamoDb.PutItemAsync(request);
            
            return account;
        }
        catch (ConditionalCheckFailedException)
        {
            throw new DomainException($"Conta com número {account.AccountNumber} não existe");
        }
        catch (AmazonDynamoDBException ex)
        {
            throw new DomainException($"Erro ao atualizar conta: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber))
            throw new ArgumentNullException(nameof(accountNumber));

        try
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "AccountNumber", new AttributeValue { S = accountNumber } }
                }
            };

            await _dynamoDb.DeleteItemAsync(request);
        }
        catch (AmazonDynamoDBException ex)
        {
            throw new DomainException($"Erro ao excluir conta: {ex.Message}", ex);
        }
    }

    private static Dictionary<string, AttributeValue> MapFromAccount(Account account)
    {
        return new Dictionary<string, AttributeValue>
        {
            { "AccountNumber", new AttributeValue { S = account.AccountNumber } },
            { "Document", new AttributeValue { S = account.Document } },
            { "Agency", new AttributeValue { S = account.Agency } },
            { "PixLimit", new AttributeValue { N = account.PixLimit.ToString() } }
        };
    }

    private static Account MapToAccount(Dictionary<string, AttributeValue> item)
    {
        if (!item.ContainsKey("Document") || !item.ContainsKey("Agency") || 
            !item.ContainsKey("AccountNumber") || !item.ContainsKey("PixLimit"))
        {
            throw new DomainException("Dados da conta incompletos no banco de dados");
        }

        return new Account(
            document: item["Document"].S,
            agency: item["Agency"].S,
            accountNumber: item["AccountNumber"].S,
            pixLimit: decimal.Parse(item["PixLimit"].N)
        );
    }
}