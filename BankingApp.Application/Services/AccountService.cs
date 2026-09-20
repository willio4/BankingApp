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

namespace BankingApp.Application.Services
{
    public class AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AccountDTO> CreateAccountAsync(CreateAccountRequestDTO requestDTO, CancellationToken cancellationToken = default)
        {
            // check if customer exist
            Customer? customer = await _customerRepository.GetByIdAsync(requestDTO.CustomerId, cancellationToken) ?? throw new InvalidCustomerException($"Customer with id: {requestDTO.CustomerId} does not exist.");

            if(string.IsNullOrEmpty(requestDTO.Currency.Trim()) || requestDTO.Currency.Length != 3) throw new ArgumentException("Currency is invalid");

            // create unique account number
            string accountNumber = GenerateUniqueAccountNumber();

            // create account
            var account = new Account(accountNumber, requestDTO.CustomerId, requestDTO.AccountType, requestDTO.Currency);

            customer.AddAccount(account);

            // add to account repository
            await _accountRepository.AddAccountAsync(account, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return account.ToDTO();
        }

        public async Task<AccountDTO?> GetAccountByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            Account? account = await _accountRepository.GetByAccountNumberAsync(accountNumber, cancellationToken);

            return account is null ? null : account.ToDTO();
        }

        public async Task<AccountDTO?> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            Account? account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            return account is null ? null : account.ToDTO();
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
    }

}