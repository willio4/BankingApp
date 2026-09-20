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
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(25);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(25);
            builder.Property(c => c.DateOfBirth).IsRequired().HasColumnType("datetimeoffset");
            builder.Property(c => c.CustomerStatus).IsRequired().HasDefaultValue(Domain.Enums.CustomerStatus.Active);
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(50);

            builder.HasMany(c => c.Accounts)
                   .WithOne()
                   .HasForeignKey(a => a.CustomerId);

            builder.HasData(
                new Customer(Guid.Parse("418586f3-7268-4035-8ae7-d2241c474876"), "Omar", "Williams", "omarwilliams@gmail.com", "111-111-1111", DateTimeOffset.Parse("10/20/1997")),
                new Customer(Guid.Parse("4bf2a680-ac31-4206-be1a-8effc3d6c427"), "Harlem", "Williams", "harlemwilliams@gmail.com", "222-222-2222", DateTimeOffset.Parse("1/2/2010")),
                new Customer(Guid.Parse("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"), "Tyler", "Rowlette", "tylerrowlette@gmail.com", "333-333-3333", DateTimeOffset.Parse("09/30/1995")),
                new Customer(Guid.Parse("afbd278c-5176-4510-9ad1-ae2acf34f80a"), "Jerey", "Rowlette", "jerseyrowlette@gmail.com", "444-444-4444", DateTimeOffset.Parse("8/31/2005"))
            );
        }
    }
}