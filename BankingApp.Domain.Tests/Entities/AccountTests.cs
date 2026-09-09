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
            Money init = new(500);
            string accountNumber = Random.Shared.NextInt64(100000000000, 1000000000000).ToString();
            Account myAccount = new(accountNumber,Enums.AccountType.Checking, "USD");

            Assert.Equal(0, myAccount.CalculateBalance());
        }


        [Fact]
        public void AccountCreateSuccessful_CalculateBalance()
        {
            // Given
            string accountNumber = Random.Shared.NextInt64(100000000000, 1000000000000).ToString();
            Account myAccount = new(accountNumber, Enums.AccountType.Checking);
            Money deposit = new(100);
            LedgerEntry deposit100 = new(myAccount.Id, deposit, Enums.EntryType.Credit, DateTime.Now);
            Money withdraw = new(30);
            LedgerEntry withdraw30 = new(myAccount.Id, withdraw, Enums.EntryType.Debit, DateTime.Now);
            // When
            myAccount.AddLedgerEntry(deposit100);
            myAccount.AddLedgerEntry(withdraw30);
            decimal expected = (deposit - withdraw).Amount;
            // Then
            Assert.Equal(expected, myAccount.CalculateBalance());
        }

        
    }
}