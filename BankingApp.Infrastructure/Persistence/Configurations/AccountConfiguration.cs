using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(12);
            builder.HasIndex(a => a.AccountNumber)
                .IsUnique();
            builder.HasMany(a => a.LedgerEntries)
                .WithOne()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(a => a.LedgerEntries)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}