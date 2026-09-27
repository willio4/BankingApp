using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs.Customer
{
    public record CustomerRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTimeOffset DateOfBirth,
        Guid UserId
    );
}