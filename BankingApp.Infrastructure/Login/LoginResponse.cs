using BankingApp.Application.DTOs.Customer;

namespace BankingApp.Application.DTOs.Login
{
    public sealed record LoginResponse(string AccessToken, string RefreshToken, CustomerResponse Customer);
}