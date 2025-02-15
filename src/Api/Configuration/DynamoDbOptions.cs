using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace FraudSys.Api.Configuration
{
    public class DynamoDbOptions
    {
        public string TableName { get; set; } = "Accounts";
        public string Region { get; set; } = "us-east-1";
        public bool LocalMode { get; set; }
        public string LocalServiceUrl { get; set; } = "http://localhost:8000";
        public int MaxRetries { get; set; }
        public string RetryMode { get; set; } = "Standard";
        public bool TTLEnabled { get; set; }
        public string TTLAttributeName { get; set; } = "TTL";

        public DynamoDbOptions()
        {
            // Os valores padrão já estão definidos nas propriedades
        }
    }
}