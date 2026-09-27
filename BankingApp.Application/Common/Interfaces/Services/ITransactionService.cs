
using BankingApp.Application.DTOs.Transaction;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse> TransferMoneyAsync(TransactionRequest requestDTO, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TransactionResponse>> GetAccountTransactionHistoryAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<TransactionResponse> DepositMoneyAsync(DepositRequest depositMoneyRequestDTO, CancellationToken cancellationToken = default);
        Task<TransactionResponse> WithdrawMoneyAsync(WithdrawRequest withdrawMoneyRequestDTO, CancellationToken cancellationToken = default);
        Task<TransactionResponse> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
    }
}