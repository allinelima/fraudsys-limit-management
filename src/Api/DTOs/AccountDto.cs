using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class AccountDto
    {
        public string Document { get; set; }
        public string Agency { get; set; }
        public string AccountNumber { get; set; }
        public decimal PixLimit { get; set; }
    }
}