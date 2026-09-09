using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Exceptions
{
    public class InvalidTransactionException : Exception
    {
        public InvalidTransactionException() : base()
        {

        }

        public InvalidTransactionException(string message) : base(message)
        {

        }
    }
}