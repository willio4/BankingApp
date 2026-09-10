using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces
{
    public interface ICustomerRepo
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
    }
}