using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Exceptions;
using BankingApp.Domain.ValueObjects;
using Xunit;

namespace BankingApp.Domain.Tests.Entities
{
    public class TransactionTests
    {
        [Fact]
        public void SuccessfulTransaction()
        {
            // build first account with $100
            Account account1 = new("account1", Enums.AccountType.Checking);
            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.Now);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)
            Account account2 = new("account2", Enums.AccountType.Savings);

            // create transaction from account1 to account2 of $25.75
            Transaction.CreateTransfer(account1, account2,new(25.75m), "Tranferring $25.75 from account1 to account2");

            Money op1 = new(100m);
            Money op2 = new(25.75m);
            // Assert
            Assert.Equal(op1.Amount - op2.Amount, account1.CalculateBalance());
            Assert.Equal(op2.Amount, account2.CalculateBalance());
        }

        [Fact]
        public void UnsuccessfulTransaction_MismatchCurrency()
        {
            // build first account with $100
            Account account1 = new("account1", Enums.AccountType.Checking);
            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.Now);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)
            Account account2 = new("account2", Enums.AccountType.Savings, "EUR");

            

            Assert.Throws<CurrencyMismatchException>(() =>
            {
                // create transaction from account1 to account2 of $25.75
                Transaction.CreateTransfer(account1, account2, new(25.75m), "Tranferring $25.75 from account1 to account2");
            });
        }

        [Fact]
        public void UnsuccessfulTransaction_InsufficientFunds()
        {
            // build first account with $100
            Account account1 = new("account1", Enums.AccountType.Checking);
            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.Now);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)
            Account account2 = new("account2", Enums.AccountType.Savings);

            Assert.Throws<InsufficientFundsException>(() =>
            {
                // create transaction from account1 to account2 of $125.75
                Transaction.CreateTransfer(account1, account2, new(125.75m), "Tranferring $125.75 from account1 to account2");
            });
        }
    }
}