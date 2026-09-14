using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class TransactionRepo(ApplicationDbContext db) : ITransactionRepository
    {
        private readonly ApplicationDbContext _db = db;

        public Task AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            _db.Transactions.Add(transaction);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            return await _db.Transactions
                .Include(t => t.Entries)
                .Where(t => t.Entries.Any(e => e.AccountID == accountId))
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Transactions
            .Include(t => t.Entries)
            .FirstOrDefaultAsync(t => t.ID == id, cancellationToken);
        }
    }
}