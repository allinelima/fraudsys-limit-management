using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FraudSys.Application.DTOs
{
    public class UpdateAccountLimitDto
    {
        public string AccountNumber { get; set; }
        public decimal NewLimit { get; set; }

        public UpdateAccountLimitDto(string accountNumber, decimal newLimit)
        {
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            NewLimit = newLimit;
        }
    }
}