using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Domain.Entities;

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

        public async Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _db.RefreshTokens.Update(refreshToken);
            return refreshToken;
        }
    }
}