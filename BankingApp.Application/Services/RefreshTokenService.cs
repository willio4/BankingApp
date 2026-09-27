using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        readonly private IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return refreshToken;
        }

        public async Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            RefreshToken updatedToken = await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshToken, cancellationToken);

            return updatedToken;
        }
    }
}