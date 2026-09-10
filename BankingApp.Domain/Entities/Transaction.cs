using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Exceptions;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Domain.Entities
{
    public class Transaction
    {
        [Key]
        public Guid ID { get; init; }
        [StringLength(250)]
        public string? Description { get; private set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; private set; }
        private readonly List<LedgerEntry> _ledgerEntries;
        public IReadOnlyCollection<LedgerEntry> Entries => _ledgerEntries.AsReadOnly();

        public Transaction(string description)
        {
            Description = description;
            _ledgerEntries = [];
            Timestamp = DateTime.Now;
            ID = Guid.NewGuid();
        }

        private Transaction()
        {
            Description = "";
            _ledgerEntries = [];
        }

        public static Transaction CreateTransfer(Account sourceAccount, Account destinationAccount, Money amount, string description)
        {
            if (sourceAccount.Currency != destinationAccount.Currency || sourceAccount.Currency != amount.Currency)
            {
                throw new CurrencyMismatchException();
            }

            if (sourceAccount.CalculateBalance() < amount.Amount)
            {
                throw new InsufficientFundsException();
            }

            Transaction transaction = new(description);

            var debitEntry = new LedgerEntry(sourceAccount.ID, amount, Enums.EntryType.Debit, DateTime.Now);
            var creditEntry = new LedgerEntry(destinationAccount.ID, amount, Enums.EntryType.Credit, DateTime.Now);

            transaction._ledgerEntries.Add(debitEntry);
            transaction._ledgerEntries.Add(creditEntry);

            sourceAccount.AddLedgerEntry(debitEntry);
            destinationAccount.AddLedgerEntry(creditEntry);

            return transaction;
        }
    }
}