namespace BankingApp.Infrastructure.Persistence.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Domain.Entities;

public class AccountRepository(ApplicationDbContext db) : IAccountRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Accounts
            .Include(a => a.LedgerEntries)
            .FirstOrDefaultAsync(a => a.ID == id, cancellationToken);
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        return await _db.Accounts
            .Include(a => a.LedgerEntries)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, cancellationToken);
    }

    public Task AddAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        _db.Accounts.Add(account);
        return Task.CompletedTask;
    }

    public Task UpdateAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        _db.Accounts.Update(account);
        return Task.CompletedTask;
    }
}