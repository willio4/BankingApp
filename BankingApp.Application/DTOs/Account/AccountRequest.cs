using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public record AccountRequest(
        Guid CustomerId,
        AccountType AccountType,
        string Currency
    );
}