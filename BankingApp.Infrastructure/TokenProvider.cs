using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs.Login;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.IdentityEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BankingApp.Application.Services
{
    public sealed class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public TokenProvider(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokens(ApplicationUser user)
        {
            var accessToken = GenerateAccessToken(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddMinutes(3),
                CreatedAt = DateTime.UtcNow
            };

            await _tokenService.AddRefreshTokenAsync(refreshToken);

            return (accessToken, refreshToken.Token);
        }

        public string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = creds
            };

            var jwt = new JsonWebTokenHandler().CreateToken(token);

            return jwt;
        }
    }
}