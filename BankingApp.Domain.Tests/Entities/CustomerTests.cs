using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Application.Services;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingApp.Domain.Tests.Entities
{
    public class CustomerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AccountService _accountService;
        private readonly ICustomerService _customerService;

        private readonly List<Customer> customers = [];
        private readonly List<Account> accounts = [];

        public CustomerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            

            _customerRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken ct) =>
                    customers.FirstOrDefault(c => c.Email == email));

            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid id, CancellationToken ct) =>
                    customers.FirstOrDefault(c => c.Id == id));

            _customerRepositoryMock
                .Setup(repo => repo.AddCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Callback<Customer, CancellationToken>((customer, ct) => customers.Add(customer))
                .Returns(Task.CompletedTask);

            _accountRepositoryMock
            .Setup(repo => repo.AddAccountAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((account, ct) => accounts.Add(account))
            .Returns(Task.CompletedTask);
            _accountRepositoryMock
                .Setup(repo => repo.GetByAccountNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string an, CancellationToken ct) =>
                    accounts.FirstOrDefault(a => a.AccountNumber == an));

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid id, CancellationToken ct) =>
                    accounts.FirstOrDefault(a => a.Id == id));

            _accountService = new AccountService(_accountRepositoryMock.Object, _customerRepositoryMock.Object, _unitOfWorkMock.Object);
            _customerService = new CustomerService(_customerRepositoryMock.Object, _unitOfWorkMock.Object, _accountService);
        }

        [Fact]
        public async Task CreateCustomer_Successfully()
        {
            CancellationTokenSource cts = new();
            Guid customerId = Guid.NewGuid();
            CreateCustomerRequestDTO customerRequestDTO = new(customerId, "Harlem", "Williams", "harwill97@aol.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            CustomerDTO? customerDTO = await _customerService.CreateCustomerAsync(customerRequestDTO, cts.Token);
            Customer customer = new(customerId, "Harlem", "Williams", "harwill97@aol.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            customerDTO.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public async Task CreateCustomer_ThrowsExistingCustomerException()
        {
            CancellationTokenSource cts = new();
            Guid customerId = Guid.NewGuid();
            CreateCustomerRequestDTO customerRequestDTO = new(customerId, "Harlem", "Williams", "harwill97@aol.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            await _customerService.CreateCustomerAsync(customerRequestDTO, cts.Token);

            Func<Task> act = async () =>
            {
                await _customerService.CreateCustomerAsync(customerRequestDTO, cts.Token);
            };

            await act.Should().ThrowAsync<ExistingCustomerException>();
        }

        [Fact]
        public async Task GetCustomer_ThrowsInvalidCustomerException()
        {
            CancellationTokenSource cts = new();
            Guid customerId = Guid.NewGuid();
            CreateCustomerRequestDTO customerRequestDTO = new(customerId, "Harlem", "Williams", "harwill97@aol.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            await _customerService.CreateCustomerAsync(customerRequestDTO, cts.Token);

            Func<Task> func = async () =>
            {
                await _customerService.GetCustomerByIdAsync(Guid.NewGuid(), cts.Token);
            };

            await func.Should().ThrowAsync<InvalidCustomerException>();
        }
    }
}
