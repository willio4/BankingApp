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
        /// <summary>
        /// Unique account identifier
        /// </summary>
        [Key]
        [Required]
        public Guid ID { get; init; } = Guid.NewGuid();

        /// <summary>
        /// 12-digit account number 
        /// </summary>
        [StringLength(12)]
        [Required]
        public string AccountNumber { get; private set; }

        /// <summary>
        /// ID of customer account belongs to
        /// </summary>
        [Required]
        public Guid CustomerID { get; private set; }

        /// <summary>
        /// Checking or Savings account enum
        /// </summary>
        [Required]
        public AccountType Type { get; private set; }

        /// <summary>
        /// Currency tied to specific account
        /// </summary>
        [Required]
        public string Currency { get; private set; }

        /// <summary>
        /// list of all ledger entries for account
        /// </summary>
        private readonly List<LedgerEntry> _ledgerEntries = [];

        /// <summary>
        /// Read-only view of ledger entries for external callers and EF Core.
        /// </summary>
        public IReadOnlyCollection<LedgerEntry> LedgerEntries => _ledgerEntries.AsReadOnly();

        /// <summary>
        /// Account constructor
        /// </summary>
        /// <param name="accountNumber">unique account number</param>
        /// <param name="customerId">id of account owner</param>
        /// <param name="type">checking or savings specifier</param>
        /// <param name="currency">currency of account</param>
        public Account(string accountNumber, Guid customerId, AccountType type, string currency = "USD")
        {
            AccountNumber = accountNumber;
            CustomerID = customerId;
            Type = type;
            Currency = currency;
        }

#pragma warning disable CS8618
        /// <summary>
        /// Account constructor for database migration
        /// </summary>
        private Account()
        {

        }
#pragma warning restore CS8618

        /// <summary>
        /// Calculates the balance of account
        /// </summary>
        /// <returns>decimal value as running balance</returns>
        public decimal CalculateBalance()
        {
            decimal balance = 0; // end balance of all ledger entries

            foreach (var entry in _ledgerEntries)
            {
                // Debit or Credit
                if (entry.Type == EntryType.Debit)
                {
                    // if type is debit, subtract from balance
                    balance -= entry.Amount.Amount;
                }
                else
                {
                    // if type is credit, add to balance
                    balance += entry.Amount.Amount;
                }
            }

            return balance;
        }

        /// <summary>
        /// Adds ledger entry to account array
        /// </summary>
        /// <param name="entry">ledger entry to be added</param>
        public void AddLedgerEntry(LedgerEntry entry)
        {
            _ledgerEntries.Add(entry);
        }
    }
}