using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.Common.Mappings;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Exceptions;
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
            Account? account = await _accountRepository.GetByIdAsync(accountId, cancellationToken) ?? throw new NullAccountException();

            // get all transactions associated with account
            IReadOnlyList<Transaction> transactions = await _transactionRepository.GetByAccountIdAsync(account.Id, cancellationToken);

            // spread transactions, map to dto then put in list and return
            return [.. transactions.Select(t => t.ToDTO())];
        }

        public async Task<TransactionDTO> TransferMoneyAsync(CreateTransactionRequestDTO requestDTO, CancellationToken cancellationToken = default)
        {
            Account? source = await _accountRepository.GetByIdAsync(requestDTO.SourceAccountId, cancellationToken) ?? throw new NullAccountException("source account does not exist");

            Account? destination = await _accountRepository.GetByIdAsync(requestDTO.DestinationAccountId, cancellationToken) ?? throw new NullAccountException("destination account does not exist");

            if (source.Currency != requestDTO.Currency || destination.Currency != requestDTO.Currency)
            {
                throw new CurrencyMismatchException("Currency mismatch between transfer request and accounts.");
            }

            Money money = new(requestDTO.Amount, requestDTO.Currency);

            Transaction transaction = Transaction.CreateTransfer(source, destination, money, requestDTO.Description);

            await _accountRepository.UpdateAccountAsync(source, cancellationToken);
            await _accountRepository.UpdateAccountAsync(destination, cancellationToken);
            await _transactionRepository.AddTransactionAsync(transaction, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return transaction.ToDTO();
        }

        public async Task<TransactionDTO> DepositMoneyAsync(DepositMoneyRequestDTO depositMoneyRequestDTO, CancellationToken cancellationToken = default)
        {
            Account? account = await _accountRepository.GetByAccountNumberAsync(depositMoneyRequestDTO.AccountNumber, cancellationToken) ?? throw new NullAccountException("Account could not be found");

            if (account.Currency != depositMoneyRequestDTO.Currency) throw new CurrencyMismatchException("Currency Mismatch");

            Money amount = new(depositMoneyRequestDTO.Amount, depositMoneyRequestDTO.Currency);

            Transaction transaction = Transaction.CreateTransfer(account, amount, $"Deposit: ${amount.Amount}, {amount.Currency}", false);
            
            await _transactionRepository.AddTransactionAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return transaction.ToDTO();
        }

        public async Task<TransactionDTO> WithdrawMoneyAsync(WithdrawMoneyRequestDTO withdrawMoneyRequestDTO, CancellationToken cancellationToken = default)
        {
            Account? account = await _accountRepository.GetByAccountNumberAsync(withdrawMoneyRequestDTO.AccountNumber, cancellationToken) ?? throw new NullAccountException("Account could not be found");

            if (account.Currency != withdrawMoneyRequestDTO.Currency) throw new CurrencyMismatchException("Currency Mismatch");

            if (account.CalculateBalance() < withdrawMoneyRequestDTO.Amount) throw new InsufficientFundsException("Insufficient funds");

            Money amount = new(withdrawMoneyRequestDTO.Amount, withdrawMoneyRequestDTO.Currency);

            Transaction transaction = Transaction.CreateTransfer(account, amount, $"Withdraw: ${amount.Amount}, {amount.Currency}", true);

            await _transactionRepository.AddTransactionAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return transaction.ToDTO();
        }

        public async Task<TransactionDTO> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            Transaction? transaction = await _transactionRepository.GetByIdAsync(transactionId, cancellationToken) ?? throw new NullTransactionException();

            return transaction.ToDTO();
        }
    }

}