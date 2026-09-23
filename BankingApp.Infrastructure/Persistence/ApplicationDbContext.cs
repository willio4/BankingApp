using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.IdentityEntities;
using BankingApp.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public ApplicationDbContext() {}

        /// <summary>
        /// Transactions Table
        /// </summary>
        public virtual DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Accounts Table
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// LedgerEntries Table
        /// </summary>
        public virtual DbSet<LedgerEntry> LedgerEntries { get; set; }

        /// <summary>
        /// Customers Table
        /// </summary>
        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}