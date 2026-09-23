using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public record CreateCustomerRequestDTO
    (
        string FirstName, 
        string LastName,
        string Email, 
        string PhoneNumber,
        DateTimeOffset DateOfBirth,
        Guid UserId
    );

    public record CustomerDTO(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTimeOffset DateOfBirth,
        CustomerStatus CustomerStatus,
        Guid UserId,
        IReadOnlyList<AccountDTO> Accounts
    );
}