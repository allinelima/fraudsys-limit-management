using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FraudSys.Api.DTOs
{
    public class AccountDto
    {
        public string Document { get; }
        public string Agency { get; }
        public string AccountNumber { get; }
        public decimal PixLimit { get; }

        public AccountDto(string document, string agency, string accountNumber, decimal pixLimit)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            Agency = agency ?? throw new ArgumentNullException(nameof(agency));
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            PixLimit = pixLimit;
        }
    }
}