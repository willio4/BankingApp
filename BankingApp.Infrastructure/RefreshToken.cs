
using BankingApp.Application.DTOs.RefreshToken;
using BankingApp.Infrastructure.IdentityEntities;

namespace BankingApp.Domain.Entities
{
    public sealed class RefreshToken
    {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsDenied => DateTime.UtcNow >= ExpiresAt || IsRevoked;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public RefreshTokenResponse ToRefreshTokenResponse()
        {
            return new RefreshTokenResponse(
            Token!,
            IsDenied,
            User
        );
        }
    }
}
