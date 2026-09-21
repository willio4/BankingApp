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
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Timestamp)
                   .IsRequired()
                   .HasColumnType("datetimeoffset");

            builder.OwnsOne(e => e.Amount, money =>
            {
                money.Property(a => a.Amount)
                     .HasColumnName("Amount")
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();

                money.Property(a => a.Currency)
                     .HasColumnName("Currency")
                     .HasMaxLength(3)
                     .HasColumnType("nvarchar(3)")
                     .IsRequired();
            });

            builder.HasOne<Account>()
                   .WithMany(a => a.LedgerEntries) 
                   .HasForeignKey(e => e.AccountId)
                   .IsRequired();
        }
    }
}