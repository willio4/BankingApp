using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository(ApplicationDbContext db) : ICustomerRepository
    {
        private readonly ApplicationDbContext _db = db;

        public Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _db.Customers.Add(customer);
            return Task.CompletedTask;
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Customers
                .FirstOrDefaultAsync(c => c.ID == id, cancellationToken);
        }

        public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _db.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }
    }
}