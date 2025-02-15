using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FraudSys.Application.DTOs
{
    public class ProcessTransactionDto
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }

        public ProcessTransactionDto(string accountNumber, decimal amount)
        {
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            Amount = amount;
        }
    }
}