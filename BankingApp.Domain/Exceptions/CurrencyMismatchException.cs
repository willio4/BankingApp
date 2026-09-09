using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Exceptions
{
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException() : base()
        {
            
        }

        public CurrencyMismatchException(string message) : base(message)
        {

        }
    }
}