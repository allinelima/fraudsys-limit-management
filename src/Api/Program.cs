using FluentValidation;
using FluentValidation.AspNetCore;
using FraudSys.Api.Configuration;
using FraudSys.Application.Services;
using FraudSys.Application.Validators;
using FraudSys.Domain.Interfaces;
using FraudSys.Infrastructure.Data.DynamoDb;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DynamoDB (usando o método AddDynamoDb)
builder.Services.AddDynamoDb(builder.Configuration);

// Add Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountDtoValidator>();

// Add application services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Configure Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Information);
});

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddCheck("dynamodb", () =>
    {
        var tableName = builder.Configuration.GetValue<string>("DynamoDB:TableName");
        if (string.IsNullOrEmpty(tableName))
            return HealthCheckResult.Unhealthy("DynamoDB:TableName não configurado no appsettings.json");
            
        return HealthCheckResult.Healthy();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Map Health Check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.Run();
