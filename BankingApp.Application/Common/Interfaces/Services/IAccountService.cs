using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.DTOs;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountDTO> CreateAccountAsync(CreateAccountRequestDTO requestDTO, CancellationToken cancellationToken = default);

        Task<AccountDTO?> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

        Task<AccountDTO?> GetAccountByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);

        Task <AccountDTO> CloseAccount(AccountDTO account, CancellationToken cancellationToken = default);
    }
}