using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Enums;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Domain.Entities
{
    public class Account
    {
        [Key]
        [Required]
        public Guid ID { get; init; } = Guid.NewGuid();

        [StringLength(12)]
        [Required]
        public string AccountNumber { get; private set; }

        [Required]
        public Guid CustomerID { get; private set; }

        [Required]
        public AccountType Type { get; private set; }
        [Required]
        public string Currency { get; private set; }

        private readonly List<LedgerEntry> _ledgerEntries = [];

        public Account(string accountNumber, Guid customerId, AccountType type, string currency = "USD")
        {
            AccountNumber = accountNumber;
            CustomerID = customerId;
            Type = type;
            Currency = currency;
        }

        // 2. ADD THIS EXACT BLOCK HERE:
        private Account()
        {
            // Left empty for EF Core proxy creation and tracking
        }

        public decimal CalculateBalance()
        {
            decimal balance = 0;
            foreach (var entry in _ledgerEntries)
            {
                if (entry.Type == EntryType.Debit)
                {
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