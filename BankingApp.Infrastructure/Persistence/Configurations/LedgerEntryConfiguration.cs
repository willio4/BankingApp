using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Infrastructure.Persistence.Configurations
{
    public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
    {
        public void Configure(EntityTypeBuilder<LedgerEntry> builder)
        {
            builder.Property(e => e.Timestamp).HasColumnType("datetimeoffset");
            builder.OwnsOne(e => e.Amount, money =>
            {
                money.Property(a => a.Amount)
                     .HasColumnName("Amount")
                     .HasColumnType("decimal(18,2)");

                money.Property(a => a.Currency)
                     .HasColumnName("Currency")
                     .HasColumnType("nvarchar(3)");
            });
        }
    }
}