using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {

        Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}