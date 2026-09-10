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
        [Key]
        public Guid ID { get; init; } = Guid.NewGuid();
        [Required]
        public Guid AccountID { get; private set; }
        [Required]
        public Money Amount { get; private set; }
        [Required]
        public EntryType Type { get; private set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; private set; }

        public LedgerEntry(Guid accountId, Money amount, EntryType type, DateTime timestamp)
        {
            AccountID = accountId;
            Amount = amount;
            Type = type;
            Timestamp = timestamp;
        }

        private LedgerEntry()
        {
            
        }
    }
}