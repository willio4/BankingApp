using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Enums;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Domain.Entities
{
    public class LedgerEntry(Guid accountId, Money amount, EntryType type, DateTime timestamp)
    {
        
        public Guid ID { get; } = Guid.NewGuid();
        public Guid AccountID { get; } = accountId;
        public Money Amount { get; } = amount;
        public EntryType Type { get; } = type;
        public DateTime Timestamp { get; } = timestamp;
    }
}