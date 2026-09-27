using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs.Account
{
    public record AccountRequest(
        Guid CustomerId,
        AccountType AccountType,
        string Currency
    );
}