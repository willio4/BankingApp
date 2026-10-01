using BankingApp.Application.DTOs.Customer;
using BankingApp.Application.DTOs.Account;
using BankingApp.Application.DTOs.Transaction;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Mappings;

public static class MappingExtensions
{
    public static CustomerResponse ToCustomerResponse(this Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            customer.DateOfBirth,
            customer.CustomerStatus.ToString(),
            customer.UserId,
            customer.Accounts.Select(a => a.ToAccountResponse()).ToList()
        );
    }

    public static AccountResponse ToAccountResponse(this Account account)
    {
        return new AccountResponse(
            account.Id,
            account.AccountNumber,
            account.CustomerId,
            account.Type.ToString(),
            account.CalculateBalance(),
            account.Currency,
            account.AccountStatus.ToString()
        );
    }

    public static TransactionResponse ToTransactionResponse(this Transaction transaction)
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