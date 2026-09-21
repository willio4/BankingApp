using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Mappings;

public static class MappingExtensions
{
    public static CustomerDTO ToDTO(this Customer customer)
    {
        return new CustomerDTO(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            customer.DateOfBirth,
            customer.CustomerStatus,
            customer.Accounts.Select(a => a.ToDTO()).ToList()
        );
    }

    public static AccountDTO ToDTO(this Account account)
    {
        return new AccountDTO(
            account.Id,
            account.AccountNumber,
            account.CustomerId,
            account.Type,
            account.CalculateBalance(),
            account.Currency,
            account.AccountStatus
        );
    }

    public static TransactionDTO ToDTO(this Transaction transaction)
    {
        return new TransactionDTO(
            transaction.Id,
            transaction.Description,
            transaction.Timestamp,
            transaction.GetAmount().Amount,
            transaction.GetAmount().Currency
        );
    }
}