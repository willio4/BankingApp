using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Mappings;

public static class MappingExtensions
{
    public static CustomerResponse ToDTO(this Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            customer.DateOfBirth,
            customer.CustomerStatus,
            customer.UserId,
            customer.Accounts.Select(a => a.ToDTO()).ToList()
        );
    }

    public static AccountResponse ToDTO(this Account account)
    {
        return new AccountResponse(
            account.Id,
            account.AccountNumber,
            account.CustomerId,
            account.Type,
            account.CalculateBalance(),
            account.Currency,
            account.AccountStatus
        );
    }

    public static TransactionResponse ToDTO(this Transaction transaction)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.Description,
            transaction.Timestamp,
            transaction.GetAmount().Amount,
            transaction.GetAmount().Currency
        );
    }
}