using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.ID);
            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(25);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(25);
            builder.Property(c => c.DateOfBirth).IsRequired().HasColumnType("datetimeoffset");
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(50);
            builder.HasMany<Account>().WithOne().HasForeignKey(a => a.CustomerID);
        }
    }
}