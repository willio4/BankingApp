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
            builder.Property(a => a.AccountStatus)
                .IsRequired()
                .HasDefaultValue(Domain.Enums.AccountStatus.Active);
            builder.HasIndex(a => a.AccountNumber)
                .IsUnique();
            builder.HasMany(a => a.LedgerEntries)
                .WithOne()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(a => a.LedgerEntries)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.HasData(
                new Account(Guid.Parse("81c8c2f1-1444-42ae-a6e5-272886a99f7e"), "693762756416", Guid.Parse("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"), Domain.Enums.AccountType.Checking, "USD", Domain.Enums.AccountStatus.Active),

                new Account(Guid.Parse("c7e97394-c25a-4f38-a54e-82e21610066d"), "990888064302", Guid.Parse("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"), Domain.Enums.AccountType.Savings, "USD", Domain.Enums.AccountStatus.Active),

                new Account(Guid.Parse("4cb0b54d-3874-464d-8acb-b3242012d81f"), "660279434684", Guid.Parse("4bf2a680-ac31-4206-be1a-8effc3d6c427"), Domain.Enums.AccountType.Checking, "USD", Domain.Enums.AccountStatus.Active),
                
                new Account(Guid.Parse("a0901605-7462-45b0-9f0c-06b58bb724e6"), "752142281253", Guid.Parse("4bf2a680-ac31-4206-be1a-8effc3d6c427"), Domain.Enums.AccountType.Savings, "USD", Domain.Enums.AccountStatus.Active),
                
                new Account(Guid.Parse("ee722d5c-fa75-49ff-a438-ae2ea86b2f18"), "416459422322", Guid.Parse("afbd278c-5176-4510-9ad1-ae2acf34f80a"), Domain.Enums.AccountType.Checking, "USD", Domain.Enums.AccountStatus.Active),
                
                new Account(Guid.Parse("1f6ec25e-6086-4747-b476-aabc217773ea"), "145967913707", Guid.Parse("afbd278c-5176-4510-9ad1-ae2acf34f80a"), Domain.Enums.AccountType.Savings, "USD", Domain.Enums.AccountStatus.Active),
                
                new Account(Guid.Parse("05781c31-1bd7-46a6-8554-b0470866c62e"), "228947707340", Guid.Parse("418586f3-7268-4035-8ae7-d2241c474876"), Domain.Enums.AccountType.Checking, "USD", Domain.Enums.AccountStatus.Active),
                
                new Account(Guid.Parse("e0b80df6-49a6-4c75-8b99-b184ad1c2fb7"), "681627673954", Guid.Parse("418586f3-7268-4035-8ae7-d2241c474876"), Domain.Enums.AccountType.Savings, "USD", Domain.Enums.AccountStatus.Active)
            );
        }
    }
}