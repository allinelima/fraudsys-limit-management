using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FraudSys.Application.DTOs
{
  public class AccountDto
    {   
    public string Document { get; set; }
    public string Agency { get; set; }
    public string AccountNumber { get; set; }
    public decimal PixLimit { get; set; }

        public AccountDto(string document, string agency, string accountNumber, decimal pixLimit)
        {
        Document = document ?? throw new ArgumentNullException(nameof(document));
        Agency = agency ?? throw new ArgumentNullException(nameof(agency));
        AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
        PixLimit = pixLimit;
        }
    }

}