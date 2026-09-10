using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using BankingApp.Domain.ValueObjects;
using Xunit;

namespace BankingApp.Domain.Tests.Entities
{
    public class AccountTests
    {
        [Fact]
        public void AccountCreateSuccessful_ZeroInitialBalance()
        {
            Customer customer1 = new("Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            Account account1 = new("000278405127", customer1.ID, Domain.Enums.AccountType.Checking);


            Assert.Equal(0, account1.CalculateBalance());
        }


        [Fact]
        public void AccountCreateSuccessful_CalculateBalance()
        {
            // Given
            Customer customer1 = new("Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            Account account1 = new("000278405127", customer1.ID, Domain.Enums.AccountType.Checking);
            Money deposit = new(100);
            LedgerEntry deposit100 = new(account1.ID, deposit, Enums.EntryType.Credit, DateTime.Now);
            Money withdraw = new(30);
            LedgerEntry withdraw30 = new(account1.ID, withdraw, Enums.EntryType.Debit, DateTime.Now);
            // When
            account1.AddLedgerEntry(deposit100);
            account1.AddLedgerEntry(withdraw30);
            decimal expected = (deposit - withdraw).Amount;
            // Then
            Assert.Equal(expected, account1.CalculateBalance());
        }


    }
}