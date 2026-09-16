using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.DTOs;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<TransactionDTO> TransferMoneyAsync(CreateTransactionRequestDTO requestDTO, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TransactionDTO>> GetAccountTransactionHistoryAsync(Guid accountId, CancellationToken cancellationToken = default);

        Task<TransactionDTO> DepositMoneyAsync(DepositMoneyRequestDTO depositMoneyRequestDTO, CancellationToken cancellationToken = default);
        Task<TransactionDTO> WithdrawMoneyAsync(WithdrawMoneyRequestDTO withdrawMoneyRequestDTO, CancellationToken cancellationToken = default);
    }
}