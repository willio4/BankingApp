using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.Common.Mappings;
using BankingApp.Application.DTOs;
using BankingApp.Application.DTOs.Account;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Exceptions;

namespace BankingApp.Application.Services
{
    public class AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AccountResponse> CreateAccountAsync(AccountRequest requestDTO, CancellationToken cancellationToken = default)
        {
            // check if customer exist
            Customer? customer = await _customerRepository.GetByIdAsync(requestDTO.CustomerId, cancellationToken) ?? throw new InvalidCustomerException($"Customer with id: {requestDTO.CustomerId} does not exist.");

            if (string.IsNullOrEmpty(requestDTO.Currency.Trim()) || requestDTO.Currency.Length != 3) throw new ArgumentException("Currency is invalid");

            // create unique account number
            string accountNumber = GenerateUniqueAccountNumber();

            // create account
            var account = new Account(accountNumber, requestDTO.CustomerId, requestDTO.AccountType, requestDTO.Currency);

            customer.AddAccount(account);

            // add to account repository
            await _accountRepository.AddAccountAsync(account, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return account.ToAccountResponse();
        }

        public async Task<AccountResponse?> GetAccountByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            Account? account = await _accountRepository.GetByAccountNumberAsync(accountNumber, cancellationToken);

            return account is null ? null : account.ToAccountResponse();
        }

        public async Task<AccountResponse?> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            Account? account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            return account is null ? null : account.ToAccountResponse();
        }

        private static string GenerateUniqueAccountNumber()
        {
            Span<char> buffer = stackalloc char[12];

            for (int i = 0; i < 12; i++)
            {
                buffer[i] = (char)('0' + Random.Shared.Next(0, 10));
            }

            return new string(buffer);
        }

        public async Task<AccountResponse> CloseAccountAsync(AccountResponse account, CancellationToken cancellationToken = default)
        {
            if (account is null) throw new NullAccountException();

            if (account.Balance != 0) throw new UnsatisfactoryAccountStandingException();

            Account? acc = await _accountRepository.GetByIdAsync(account.Id, cancellationToken);

            if (acc is null) throw new NullAccountException();

            acc.AccountStatus = Domain.Enums.AccountStatus.Closed;

            await _accountRepository.UpdateAccountAsync(acc, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            acc = await _accountRepository.GetByIdAsync(account.Id, cancellationToken);

            return acc!.ToAccountResponse();
        }
    }

}