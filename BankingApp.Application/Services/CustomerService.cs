using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.Common.Mappings;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;
using BankingApp.Domain.Exceptions;

namespace BankingApp.Application.Services
{
    public partial class CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IAccountService accountService) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAccountService _accountService = accountService;

        public async Task<CustomerDTO> CreateCustomerAsync(CreateCustomerRequestDTO requestDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(requestDTO);

            if (string.IsNullOrWhiteSpace(requestDTO.FirstName) ||
                string.IsNullOrWhiteSpace(requestDTO.LastName) ||
                string.IsNullOrWhiteSpace(requestDTO.Email) ||
                string.IsNullOrWhiteSpace(requestDTO.PhoneNumber))
                throw new ArgumentException("Customer details cannot be empty");

            if (!MailAddress.TryCreate(requestDTO.Email, out _)) throw new ArgumentException("Invalid email address");

            Regex regex = MyRegex1();

            if (!regex.IsMatch(requestDTO.PhoneNumber)) throw new ArgumentException("Invalid phone number");

            Customer? existingCustomer = await _customerRepository
                .GetByEmailAsync(requestDTO.Email, cancellationToken);

            if (existingCustomer is not null) throw new ExistingCustomerException("User with email already exist");

            Customer customer = new(Guid.NewGuid(), requestDTO.FirstName, requestDTO.LastName, requestDTO.Email, requestDTO.PhoneNumber, requestDTO.DateOfBirth, requestDTO.UserId);

            await _customerRepository.AddCustomerAsync(customer, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.ToDTO();
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository
                .GetByIdAsync(id, cancellationToken);

            return customer == null ? throw new InvalidCustomerException("Customer does not exist") : customer.ToDTO();
        }


        public async Task<AccountDTO> OpenAccountAsync(CreateAccountRequestDTO accountRequestDTO, CancellationToken cancellationToken)
        {
            // check if customer id is valid
            Customer? customer = await _customerRepository.GetByIdAsync(accountRequestDTO.CustomerId, cancellationToken) ?? throw new InvalidCustomerException();

            // create account from account request
            AccountDTO accountDTO = await _accountService.CreateAccountAsync(accountRequestDTO, cancellationToken);

            return accountDTO;
        }

        [GeneratedRegex(@"^\d{3}-?\d{3}-?\d{4}$")]
        private static partial Regex MyRegex1();

        public async Task<CustomerDTO?> UpdateCustomer(CustomerDTO previous, CreateCustomerRequestDTO updated, CancellationToken cancellationToken = default)
        {
            CancellationTokenSource cts = new();
            Customer? customer = await _customerRepository.GetByIdAsync(previous.Id, cts.Token);

            if (customer is not null)
            {
                customer.FirstName = updated.FirstName;
                customer.LastName = updated.LastName;
                customer.Email = updated.Email;
                customer.PhoneNumber = updated.PhoneNumber;
                customer.DateOfBirth = updated.DateOfBirth;

                await _customerRepository.UpdateCustomerAsync(customer, cts.Token);
                await _unitOfWork.SaveChangesAsync(cts.Token);
            }

            return customer!.ToDTO();
        }

        public async Task<CustomerDTO> DeleteCustomer(CustomerDTO customer, CancellationToken cancellationToken = default)
        {
            if(customer is null) throw new InvalidCustomerException();
            Customer? update = await _customerRepository.GetByIdAsync(customer.Id, cancellationToken);

            update!.CustomerStatus = CustomerStatus.Closed;
            await _customerRepository.UpdateCustomerAsync(update, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            update = await _customerRepository.GetByIdAsync(customer.Id, cancellationToken);

            return update!.ToDTO();
        }
    }
}