using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    }
}