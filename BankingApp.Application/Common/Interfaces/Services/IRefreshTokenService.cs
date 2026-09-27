using BankingApp.Application.DTOs.Login;
using BankingApp.Application.DTOs.RefreshToken;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<string> GenerateAccessToken(LoginRequest request, CancellationToken cancellationToken = default);
        Task<string> RefreshAccessToken(RefreshTokenRequest request, CancellationToken cancellationToken = default);
        Task<string> GenerateRefreshToken(LoginRequest request, CancellationToken cancellationToken = default);
        Task<RefreshTokenResponse?> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);
        Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    }
}