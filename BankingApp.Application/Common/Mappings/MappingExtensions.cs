using BankingApp.Application.DTOs.User;
using BankingApp.Application.DTOs.Customer;
using BankingApp.Application.DTOs.Login;
using BankingApp.Application.DTOs.Account;
using BankingApp.Application.DTOs.Transaction;
using BankingApp.Application.DTOs.RefreshToken;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;
using BankingApp.Infrastructure.IdentityEntities;

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
            customer.CustomerStatus,
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
            account.Type,
            account.CalculateBalance(),
            account.Currency,
            account.AccountStatus
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

    public static UserResponse ToUserResponse(this ApplicationUser user)
    {
        return new UserResponse(
            user.FirstName!,
            user.LastName!,
            user.Email!,
            user.PhoneNumber!
        );
    }

    public static RefreshTokenResponse ToRefreshTokenResponse(this RefreshToken refreshToken)
    {
        return new RefreshTokenResponse(
            refreshToken.Token!,
            refreshToken.IsDenied,
            refreshToken.User
        );
    }
}