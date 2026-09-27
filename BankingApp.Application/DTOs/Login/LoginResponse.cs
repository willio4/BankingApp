namespace BankingApp.Application.DTOs
{
    public sealed record LoginResponse(string AccessToken, string RefreshToken);
}