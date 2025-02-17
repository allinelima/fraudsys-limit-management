using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Account
    {
        public string Document { get; set; }  
        public string Agency { get; set; }    
        public string AccountNumber { get; set; } 
        public decimal PixLimit { get; set; }  


        public Account(string document, string agency, string accountNumber, decimal pixLimit)
        {
            Document = document;
            Agency = agency;
            AccountNumber = accountNumber;
            PixLimit = pixLimit;
        }
    }
}