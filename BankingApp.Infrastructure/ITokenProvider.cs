using BankingApp.Infrastructure.IdentityEntities;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface ITokenProvider
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokens(ApplicationUser user);
        string GenerateAccessToken(ApplicationUser user);    
    }
}