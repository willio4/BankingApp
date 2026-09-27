using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.DTOs;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest requestDTO, CancellationToken cancellationToken = default);

        Task<CustomerResponse?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<CustomerResponse?> UpdateCustomerAsync(CustomerResponse previous, CustomerRequest updated, CancellationToken cancellationToken = default);

        Task<AccountResponse> OpenAccountAsync(AccountRequest accountRequestDTO, CancellationToken cancellationToken = default);

        Task<CustomerResponse> DeleteCustomerAsync(CustomerResponse customer, CancellationToken cancellationToken = default);
    }
}