namespace BankingApp.Application.DTOs.Login
{
    public sealed record LoginResponse(string AccessToken, string RefreshToken);
}