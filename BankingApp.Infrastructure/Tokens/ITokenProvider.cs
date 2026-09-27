using BankingApp.Infrastructure.IdentityEntities;

namespace BankingApp.Infrastructure.Tokens
{
    public interface ITokenProvider
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokens(ApplicationUser user);
        string GenerateAccessToken(ApplicationUser user);    
    }
}