using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public record AccountResponse(
        Guid Id,
        string AccountNumber,
        Guid CustomerId,
        AccountType Type,
        decimal Balance,
        string Currency,
        AccountStatus AccountStatus
    );
}