using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace FraudSys.Infrastructure.Data.DynamoDb;

public class AccountRepository : IAccountRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName;

    public AccountRepository(IAmazonDynamoDB dynamoDb, IConfiguration configuration)
    {
        _dynamoDb = dynamoDb;
        _tableName = configuration["DynamoDB:TableName"];
    }

    public async Task<Account> GetByAccountNumberAsync(string accountNumber)
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

    public async Task AddAsync(Account account)
    {
        var item = MapFromAccount(account);
        
        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item,
            ConditionExpression = "attribute_not_exists(AccountNumber)"
        };

        await _dynamoDb.PutItemAsync(request);
    }

    public async Task UpdateAsync(Account account)
    {
        var item = MapFromAccount(account);
        
        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item,
            ConditionExpression = "attribute_exists(AccountNumber)"
        };

        await _dynamoDb.PutItemAsync(request);
    }

    public async Task DeleteAsync(string accountNumber)
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
        return new Account(
            document: item["Document"].S,
            agency: item["Agency"].S,
            accountNumber: item["AccountNumber"].S,
            pixLimit: decimal.Parse(item["PixLimit"].N)
        );
    }
}