using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Exceptions;
using BankingApp.Domain.ValueObjects;
using Xunit;

namespace BankingApp.Domain.Tests.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void AddingSameCurrencyTest_Successful()
        {
            Money op1 = new(50, "USD");
            Money op2 = new(50, "USD");
            Money result = op1 + op2;

            Assert.Equal(op1 + op2, result);
        }

        [Fact]
        public void AddingDifferentCurrencyTest_WrongCurrency()
        {
            // Given
            Money op1 = new(50, "USD");
            Money op2 = new(50, "EUR");
            

            // When

            // Then
            Assert.Throws<CurrencyMismatchException>(() =>
            {
                Money result = op1 + op2;
            });
        }

        [Fact]
        public void SubtractingSameCurrencyTest_Successful()
        {
            Money op1 = new(50, "USD");
            Money op2 = new(50, "USD");
            Money result = op1 - op2;

            Assert.Equal(op1 - op2, result);
        }

        [Fact]
        public void SubtractingSameCurrencyTest_InsufficientFunds()
        {
            // Given
            Money op1 = new(50, "USD");
            Money op2 = new(51, "USD");


            // When

            // Then
            Assert.Throws<InsufficientFundsException>(() =>
            {
                Money result = op1 - op2;
            });
        }

        [Fact]
        public void MultiplyingSameCurrencyTest_InvalidTransaction()
        {
            // Given
            Money op1 = new(50, "USD");
            Money op2 = new(51, "USD");


            // When

            // Then
            Assert.Throws<InvalidTransactionException>(() =>
            {
                Money result = op1 * op2;
            });
        }

        [Fact]
        public void DividingSameCurrencyTest_InvalidTransaction()
        {
            // Given
            Money op1 = new(50, "USD");
            Money op2 = new(51, "USD");


            // When

            // Then
            Assert.Throws<InvalidTransactionException>(() =>
            {
                Money result = op1 / op2;
            });
        }

    }
}