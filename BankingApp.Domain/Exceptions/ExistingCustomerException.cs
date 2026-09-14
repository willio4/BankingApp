using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Exceptions
{
    public class ExistingCustomerException : Exception
    {
        public ExistingCustomerException() : base()
        {
            
        }

        public ExistingCustomerException(string message) : base(message)
        {

        }
    }
}