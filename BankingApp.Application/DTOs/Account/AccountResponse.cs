using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs.Account
{
    public record AccountResponse(
        Guid Id,
        string AccountNumber,
        Guid CustomerId,
        string Type,
        decimal Balance,
        string Currency,
        string AccountStatus
    );
}