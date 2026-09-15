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
        /// <summary>
        /// unique transaction identifier
        /// </summary>
        [Key]
        public Guid ID { get; init; }
        /// <summary>
        /// user given description of transaction
        /// </summary>
        [StringLength(250)]
        public string Description { get; private set; }
        /// <summary>
        /// timestamp of transaction
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; private set; }
        /// <summary>
        /// record of ledger entries of transaction
        /// </summary>
        private readonly List<LedgerEntry> _ledgerEntries;
        public IReadOnlyCollection<LedgerEntry> Entries => _ledgerEntries.AsReadOnly();

        /// <summary>
        /// Transaction constructor
        /// </summary>
        /// <param name="description">brief description detailing transaction</param>
        public Transaction(string description)
        {
            Description = description;
            _ledgerEntries = [];
            Timestamp = DateTime.UtcNow;
            ID = Guid.NewGuid();
        }

#pragma warning disable CS8618
        private Transaction()
        {

        }
#pragma warning restore CS8618

        /// <summary>
        /// Creates transfer between two accounts
        /// </summary>
        /// <param name="sourceAccount">account sending money</param>
        /// <param name="destinationAccount">account accepting money</param>
        /// <param name="amount">amount to transfer between accounts</param>
        /// <param name="description">brief description of transaction</param>
        /// <returns>transaction object</returns>
        /// <exception cref="CurrencyMismatchException">source and destination account currencys don't match</exception>
        /// <exception cref="InsufficientFundsException">source account sending mor than available</exception>
        public static Transaction CreateTransfer(Account sourceAccount, Account destinationAccount, Money amount, string description)
        {
            // if currency between accounts dont match throw
            if (sourceAccount.Currency != destinationAccount.Currency || sourceAccount.Currency != amount.Currency)
            {
                throw new CurrencyMismatchException();
            }
            // if source is sending more than what they have throw
            if (sourceAccount.CalculateBalance() < amount.Amount)
            {
                throw new InsufficientFundsException();
            }

            Transaction transaction = new(description);

            // creation of ledger entries
            var debitEntry = new LedgerEntry(sourceAccount.Id, amount, Enums.EntryType.Debit, DateTime.UtcNow);
            var creditEntry = new LedgerEntry(destinationAccount.Id, amount, Enums.EntryType.Credit, DateTime.UtcNow);

            // add entries to transaction history
            transaction._ledgerEntries.Add(debitEntry);
            transaction._ledgerEntries.Add(creditEntry);

            // add entry to account ledger entries
            sourceAccount.AddLedgerEntry(debitEntry);
            destinationAccount.AddLedgerEntry(creditEntry);

            return transaction;
        }

        public Money GetAmount()
        {
            if (_ledgerEntries.Count == 0) throw new InvalidOperationException("Cannot resolve amount for a transaction with no ledger entries.");

            LedgerEntry source = _ledgerEntries[0];
            return source.Amount;
        }
    }

}