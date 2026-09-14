using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs
{
    public record CreateCustomerRequestDTO
    (
        Guid Id,
        string FirstName, 
        string LastName,
        string Email, 
        string PhoneNumber,
        DateTime DateOfBirth
    );

    public record CustomerDTO(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        List<Account> Accounts
    );
}