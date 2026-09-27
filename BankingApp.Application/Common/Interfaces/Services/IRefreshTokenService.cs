using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default);

        Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    }
}