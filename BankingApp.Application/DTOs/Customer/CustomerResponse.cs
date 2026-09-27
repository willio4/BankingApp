using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public record CustomerResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTimeOffset DateOfBirth,
        CustomerStatus CustomerStatus,
        Guid UserId,
        IReadOnlyList<AccountResponse> Accounts
    );
}