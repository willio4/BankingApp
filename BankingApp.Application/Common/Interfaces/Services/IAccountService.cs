using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.DTOs;
using BankingApp.Application.DTOs.Account;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAccountAsync(AccountRequest requestDTO, CancellationToken cancellationToken = default);

        Task<AccountResponse?> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

        Task<AccountResponse?> GetAccountByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);

        Task<AccountResponse> CloseAccountAsync(AccountResponse account, CancellationToken cancellationToken = default);
    }
}