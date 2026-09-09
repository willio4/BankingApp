using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Exceptions;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Domain.Entities
{
    public class Transaction
    {
        private readonly List<LedgerEntry> _ledgerEntries = [];
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public IReadOnlyCollection<LedgerEntry> Entries => _ledgerEntries.AsReadOnly();

        public Transaction(string description)
        {
            Description = description;
        }

        public static Transaction CreateTransfer(Account sourceAccount, Account destinationAccount, Money amount, string description)
        {
            if(sourceAccount.Currency != destinationAccount.Currency || sourceAccount.Currency != amount.Currency)
            {
                throw new CurrencyMismatchException();
            }

            if(sourceAccount.CalculateBalance() < amount.Amount)
            {
                throw new InsufficientFundsException();
            }

            Transaction transaction = new(description);

            var debitEntry = new LedgerEntry(sourceAccount.Id, amount, Enums.EntryType.Debit, DateTime.Now);
            var creditEntry = new LedgerEntry(destinationAccount.Id, amount, Enums.EntryType.Credit, DateTime.Now);

            transaction._ledgerEntries.Add(debitEntry);
            transaction._ledgerEntries.Add(creditEntry);

            sourceAccount.AddLedgerEntry(debitEntry);
            destinationAccount.AddLedgerEntry(creditEntry);

            return transaction;
        }
    }
}