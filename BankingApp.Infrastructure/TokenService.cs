using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.Common.Mappings;
using BankingApp.Application.DTOs.Login;
using BankingApp.Application.DTOs.RefreshToken;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.IdentityEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BankingApp.Application.Services
{
    public class TokenService : ITokenService
    {
        readonly private IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public TokenService(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return refreshToken;
        }

        public async Task<string> GenerateAccessToken(LoginRequest request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new("permission", "orders:read"),
                new("permission", "orders:write")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds
            };

            var jwt = new JsonWebTokenHandler().CreateToken(token);

            return jwt;
        }

        public async Task<string> RefreshAccessToken(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            RefreshTokenResponse? refreshToken = await GetRefreshTokenAsync(request, cancellationToken);

            if (refreshToken is null) return null!;

            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(refreshToken.User.Email!, cancellationToken);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new("permission", "orders:read"),
                new("permission", "orders:write")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds
            };

            var jwt = new JsonWebTokenHandler().CreateToken(token);

            return jwt;
        }

        public async Task<string> GenerateRefreshToken(LoginRequest request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user!.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                CreatedAt = DateTime.UtcNow
            };

            await AddRefreshTokenAsync(refreshToken, cancellationToken);

            return refreshToken.Token;
        }

        public async Task<RefreshTokenResponse?> GetRefreshTokenAsync(RefreshTokenRequest refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken? token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken.RefreshToken, cancellationToken);

            if (token is null) return null;
            if(token.ExpiresAt < DateTime.UtcNow || token.Token is not null)
            {
                await RevokeTokenAsync(token.Token!, cancellationToken);
                return null;
            }
                

            return token.ToRefreshTokenResponse();
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            RefreshToken? token = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken, cancellationToken);

            if(token is null) return false;
            if(token.IsRevoked) return false;
            
            token.IsRevoked = true;
            
            await UpdateRefreshToken(token, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            RefreshToken updatedToken = await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshToken, cancellationToken);

            return updatedToken;
        }
    }
}