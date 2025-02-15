using Amazon.Extensions.NETCore.Setup;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FraudSys.Api.Configuration;

namespace FraudSys.Api.Configuration
{
     public static class DynamoDbConfig
    {
        public static IServiceCollection AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Obter as opções do AWS a partir do appsettings
            var awsOptions = configuration.GetAWSOptions("AWS");

            // Adicionar o serviço do DynamoDB
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();

            // Configurar o DynamoDB com a tabela e outros parâmetros
            var dynamoDbOptions = configuration.GetSection("DynamoDB");
            services.Configure<FraudSys.Api.Configuration.DynamoDbOptions>(dynamoDbOptions);

            return services;
        }
    }
}
