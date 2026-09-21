using System;

namespace BankingApp.Domain.Exceptions;

public class UnsatisfactoryAccountStandingException : Exception
{
    public UnsatisfactoryAccountStandingException() : base()
    {
        
    }

    public UnsatisfactoryAccountStandingException(string message) : base(message)
    {
        
    }
}
