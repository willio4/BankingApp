using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Enums;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Domain.Entities
{
    public class Account(string AccountNumber, AccountType Type, string Currency)
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string AccountNumber { get; } = AccountNumber;
        public Guid CustomerId { get; } = Guid.NewGuid();
        public AccountType Type { get; } = Type;
        public string Currency { get; } = Currency;
        private readonly List<LedgerEntry> _ledgerEntries = [];

        public decimal CalculateBalance()
        {
            decimal balance = 0;
            foreach(var entry in _ledgerEntries)
            {
                if(entry.Type == EntryType.Debit) {
                    balance -= entry.Amount.Amount;
                } 
                else
                {
                    balance += entry.Amount.Amount;
                }
            }
            return balance;
        }

        public void AddLedgerEntry(LedgerEntry entry)
        {
            _ledgerEntries.Add(entry);
        }
    }
}