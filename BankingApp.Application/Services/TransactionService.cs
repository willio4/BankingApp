using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Application.Services
{
    public class TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, IUnitOfWork unitOfWork) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<TransactionDTO>> GetAccountTransactionHistoryAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            // check account exist
            Account? account = await _accountRepository.GetByIdAsync(accountId, cancellationToken) ?? throw new InvalidOperationException();

            // get all transactions associated with account
            IReadOnlyList<Transaction> transactions = await _transactionRepository.GetByAccountIdAsync(account.ID, cancellationToken);

            // spread transactions, map to dto then put in list and return
            return [..transactions.Select(MapToDTO)];
        }

        public async Task<TransactionDTO> TransferMoneyAsync(TransferRequestDTO requestDTO, CancellationToken cancellationToken = default)
        {
            Account? source = await _accountRepository.GetByIdAsync(requestDTO.SourceAccountId, cancellationToken) ?? throw new InvalidOperationException("source account does not exist");

            Account? destination = await _accountRepository.GetByIdAsync(requestDTO.DestinationAccountId, cancellationToken) ?? throw new InvalidOperationException("destination account does not exist");

            if (source.Currency != requestDTO.Currency || destination.Currency != requestDTO.Currency)
            {
                throw new InvalidOperationException("Currency mismatch between transfer request and accounts.");
            }

            Money money = new(requestDTO.Amount, requestDTO.Currency);

            Transaction transaction = Transaction.CreateTransfer(source, destination, money, requestDTO.Description);

            await _accountRepository.UpdateAccountAsync(source, cancellationToken);
            await _accountRepository.UpdateAccountAsync(destination, cancellationToken);
            await _transactionRepository.AddTransactionAsync(transaction, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToDTO(transaction);

        }

        private static TransactionDTO MapToDTO(Transaction transaction)
        {
            Money amount = transaction.GetAmount();
            return new TransactionDTO(transaction.ID, transaction.Description, transaction.Timestamp, amount.Amount, amount.Currency);
        }
    }
    
}