using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.DTOs;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateCustomerAsync(CreateCustomerRequestDTO requestDTO, CancellationToken cancellationToken = default);

        Task<CustomerDTO?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<CustomerDTO?> UpdateCustomer(CustomerDTO previous, CreateCustomerRequestDTO updated, CancellationToken cancellationToken = default);

        Task<AccountDTO> OpenAccountAsync(CreateAccountRequestDTO accountRequestDTO, CancellationToken cancellationToken);
    }
}