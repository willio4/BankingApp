using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Exceptions
{
    public class NullAccountException : Exception
    {
        public NullAccountException() : base()
        {
            
        }

        public NullAccountException(string message) : base(message)
        {
            
        }
    }
}