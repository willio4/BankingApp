using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base()
        {

        }

        public InsufficientFundsException(string message) : base(message)
        {

        }
    }
}