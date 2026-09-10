using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Exceptions;

namespace BankingApp.Domain.ValueObjects
{
    public record Money
    {
        [Required(ErrorMessage = "Amount can not be blank")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; }
        [StringLength(3)]
        public string Currency { get; }
        public Money(decimal amount, string currency = "USD")
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money operator +(Money op1, Money op2)
        {
            if(op1.Currency != op2.Currency)
            {
                throw new CurrencyMismatchException();
            } 

            return new Money(op1.Amount + op2.Amount, op1.Currency);
        }

        public static Money operator -(Money op1, Money op2)
        {
            if (op1.Currency != op2.Currency)
            {
                throw new CurrencyMismatchException();
            }
            if(op1.Amount < op2.Amount)
            {
                throw new InsufficientFundsException();
            }

            return new Money(op1.Amount - op2.Amount, op1.Currency);
        }

        public static Money operator *(Money op1, Money op2)
        {
            throw new InvalidTransactionException();
        }
        public static Money operator /(Money op1, Money op2)
        {
            throw new InvalidTransactionException();
        }
    }
}