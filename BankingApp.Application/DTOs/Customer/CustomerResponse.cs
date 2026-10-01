using BankingApp.Application.DTOs.Account;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;


namespace BankingApp.Application.DTOs.Customer
{
    public record CustomerResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTimeOffset DateOfBirth,
        string CustomerStatus,
        Guid UserId,
        IReadOnlyList<AccountResponse> Accounts
    );
}