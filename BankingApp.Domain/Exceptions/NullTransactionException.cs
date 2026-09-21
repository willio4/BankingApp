using System;

namespace BankingApp.Domain.Exceptions;

public class NullTransactionException : Exception
{
    public NullTransactionException() : base()
    {
        
    }

    public NullTransactionException(string message) : base(message)
    {
        
    }
}
