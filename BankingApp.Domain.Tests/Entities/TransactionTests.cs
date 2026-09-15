using System;
using System.Collections.Generic;
using System.Data.Common;
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
            Guid id = Guid.NewGuid();
            Customer customer1 = new(id, "Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));
            Customer customer2 = new(id, "Jersey", "Rowlette", "jerzrowlette@gmail.com", "222-222-2222", DateTime.Parse("09/30/1995"));

            Account account1 = new("000278405127", customer1.Id, Domain.Enums.AccountType.Checking);
            Account account2 = new("010470247583", customer2.Id, Domain.Enums.AccountType.Checking);

            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.UtcNow);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)

            // create transaction from account1 to account2 of $25.75
            Transaction.CreateTransfer(account1, account2, new(25.75m), "Tranferring $25.75 from account1 to account2");

            Money op1 = new(100m);
            Money op2 = new(25.75m);
            // Assert
            Assert.Equal(op1.Amount - op2.Amount, account1.CalculateBalance());
            Assert.Equal(op2.Amount, account2.CalculateBalance());
        }

        [Fact]
        public void UnsuccessfulTransaction_MismatchCurrency()
        {
            Guid id = Guid.NewGuid();
            // build first account with $100
            Customer customer1 = new(id, "Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));
            Customer customer2 = new(id, "Jersey", "Rowlette", "jerzrowlette@gmail.com", "222-222-2222", DateTime.Parse("09/30/1995"));

            Account account1 = new("000278405127", customer1.Id, Domain.Enums.AccountType.Checking);
            Account account2 = new("010470247583", customer2.Id, Domain.Enums.AccountType.Checking, "EUR");
            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.UtcNow);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)



            Assert.Throws<CurrencyMismatchException>(() =>
            {
                // create transaction from account1 to account2 of $25.75
                Transaction.CreateTransfer(account1, account2, new(25.75m), "Tranferring $25.75 from account1 to account2");
            });
        }

        [Fact]
        public void UnsuccessfulTransaction_InsufficientFunds()
        {
            Guid id = Guid.NewGuid();
            // build first account with $100
            Customer customer1 = new(id, "Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));
            Customer customer2 = new(id, "Jersey", "Rowlette", "jerzrowlette@gmail.com", "222-222-2222", DateTime.Parse("09/30/1995"));

            Account account1 = new("000278405127", customer1.Id, Domain.Enums.AccountType.Checking);
            Account account2 = new("010470247583", customer2.Id, Domain.Enums.AccountType.Checking);

            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.UtcNow);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)

            Assert.Throws<InsufficientFundsException>(() =>
            {
                // create transaction from account1 to account2 of $125.75
                Transaction.CreateTransfer(account1, account2, new(125.75m), "Tranferring $125.75 from account1 to account2");
            });
        }
    }
}