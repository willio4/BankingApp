using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;
using BankingApp.Domain.Exceptions;

namespace BankingApp.Application.Services
{
    public partial class CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, AccountService accountService) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAccountService _accountService = accountService;

        public async Task<CustomerDTO> CreateCustomerAsync(CreateCustomerRequestDTO requestDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(requestDTO);

            if (string.IsNullOrEmpty(requestDTO.FirstName.Trim()) || string.IsNullOrEmpty(requestDTO.LastName.Trim()) || string.IsNullOrEmpty(requestDTO.Email.Trim()) || string.IsNullOrEmpty(requestDTO.PhoneNumber.Trim())) throw new ArgumentException("Customer details cannot be null");
            
            if(!MailAddress.TryCreate(requestDTO.Email, out _)) throw new ArgumentException("Invalid email address");

            Regex regex = MyRegex1();
            
            if(!regex.IsMatch(requestDTO.PhoneNumber)) throw new ArgumentException("Invalid phone number");

            Customer? existingCustomer = await _customerRepository
                .GetByEmailAsync(requestDTO.Email, cancellationToken);

            if (existingCustomer is not null) throw new ExistingCustomerException("User with email already exist");

            Customer customer = new(requestDTO.Id, requestDTO.FirstName, requestDTO.LastName, requestDTO.Email, requestDTO.PhoneNumber, requestDTO.DateOfBirth);

            await _customerRepository.AddCustomerAsync(customer, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToDTO(customer);
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

            return customer == null ? throw new InvalidCustomerException("Customer does not exist") : MapToDTO(customer);
        }

        public static CustomerDTO MapToDTO(Customer customer)
        {
            return new CustomerDTO(customer.Id, customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber, customer.DateOfBirth, customer.GetAccounts());
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
    }
}