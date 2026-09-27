using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _db;

        public RefreshTokenRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            _db.RefreshTokens.Add(refreshToken);
            return Task.CompletedTask;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return await _db.RefreshTokens
                .Where(rt => rt.IsDenied == false)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);
        }

        public async Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _db.RefreshTokens.Update(refreshToken);
            return refreshToken;
        }
    }
}