using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs
{
    public record CreateCustomerRequestDTO
    (
        string FirstName, 
        string LastName,
        string Email, 
        string PhoneNumber,
        DateTimeOffset DateOfBirth
    );

    public record CustomerDTO(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTimeOffset DateOfBirth,
        List<Account> Accounts
    );
}