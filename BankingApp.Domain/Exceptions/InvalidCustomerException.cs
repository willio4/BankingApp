using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Exceptions
{
    public class InvalidCustomerException : Exception
    {
        public InvalidCustomerException() : base()
        {
            
        }

        public InvalidCustomerException(string message) : base(message)
        {

        }
    }
}