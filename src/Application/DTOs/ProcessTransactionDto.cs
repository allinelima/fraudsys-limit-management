using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProcessTransactionDto
    {
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
    }
}