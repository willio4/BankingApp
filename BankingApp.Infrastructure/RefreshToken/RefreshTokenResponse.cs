using BankingApp.Infrastructure.IdentityEntities;

namespace BankingApp.Application.DTOs.RefreshToken
{
    public record RefreshTokenResponse(string RefreshToken, bool IsDenied, ApplicationUser User);
}