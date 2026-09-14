using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default);

        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}