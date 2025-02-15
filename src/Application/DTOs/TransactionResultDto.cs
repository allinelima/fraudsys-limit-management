using System;

namespace FraudSys.Application.DTOs
{
    public class TransactionResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal NewBalance { get; set; }

        public TransactionResultDto(bool success, string message, decimal newBalance)
        {
            Success = success;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            NewBalance = newBalance;
        }
    }
}