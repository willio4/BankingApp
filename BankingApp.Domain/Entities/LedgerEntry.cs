using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Enums;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Domain.Entities
{

    public class LedgerEntry
    {
        /// <summary>
        /// ledger entry unique identifier
        /// </summary>
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        /// <summary>
        /// id of account ledger entry belongs to
        /// </summary>
        [Required]
        public Guid AccountId { get; private set; }
        /// <summary>
        /// money object associated with ledger entry
        /// </summary>
        [Required]
        public Money Amount { get; private set; }
        /// <summary>
        /// credit or debit
        /// </summary>
        [Required]
        public EntryType Type { get; private set; }
        /// <summary>
        /// date and time of ledger entry
        /// </summary>
        [Required]
        public DateTimeOffset Timestamp { get; private set; }

        public Guid TransactionId { get; private set; }

        /// <summary>
        /// LedgerEntry constructor
        /// </summary>
        /// <param name="accountId">id of associated account</param>
        /// <param name="amount">money object</param>
        /// <param name="type">debit or credit</param>
        /// <param name="timestamp">timestamp of ledger entry</param>
        public LedgerEntry(Guid accountId, Money amount, EntryType type, DateTimeOffset timestamp)
        {
            AccountId = accountId;
            Amount = amount;
            Type = type;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Empty constructor for database migration
        /// </summary>
#pragma warning disable CS8618
        private LedgerEntry()
        {
        }
#pragma warning restore CS8618
    }
}