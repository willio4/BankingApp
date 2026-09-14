using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;
using BankingApp.Domain.Exceptions;

namespace BankingApp.Application.Services
{
    public class CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CustomerDTO> CreateCustomerAsync(CreateCustomerRequestDTO requestDTO, CancellationToken cancellationToken = default)
        {
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


    }
}