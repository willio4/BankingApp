using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Application.Services;
using BankingApp.Domain.Entities;
using BankingApp.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingApp.Domain.Tests.Entities
{
    public class AccountTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;

        private readonly CustomerService _customerService;
        private readonly AccountService _accountService;
        private readonly List<Customer> customers = [];
        private readonly List<Account> accounts = [];

        public AccountTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
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
            _customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _accountService
                );

        }


        [Fact]
        public async Task AccountCreate_Successful()
        {
            // Arrange
            CancellationTokenSource cts = new();
            Customer customer = new(
                Guid.NewGuid(),
                "Harlem",
                "Williams",
                "harwill97@gmail.com",
                "111-111-1111",
                DateTimeOffset.Parse("10/20/1997"),
                Guid.NewGuid()
            );

            customers.Add(customer);

            CreateAccountRequestDTO accountRequestDTO = new(customer.Id, Enums.AccountType.Checking, "USD");

            // Act
            AccountDTO accountDTO = await _customerService.OpenAccountAsync(accountRequestDTO, cts.Token);

            // Assert
            List<Account> accounts = customer.Accounts.ToList();

            accounts.Should().ContainEquivalentOf(accountDTO, options => options
                .Excluding(a => a.Balance)
            );
        }


        [Fact]
        public async Task AccountCreateSuccessful_CalculateBalance()
        {
            CancellationTokenSource cts = new();
            Customer customer = new(
                Guid.NewGuid(),
                "Harlem",
                "Williams",
                "harwill97@gmail.com",
                "111-111-1111",
                DateTimeOffset.Parse("10/20/1997"),
                Guid.NewGuid()
            );

            customers.Add(customer);

            CreateAccountRequestDTO accountRequestDTO = new(customer.Id, Enums.AccountType.Checking, "USD");

            AccountDTO accountDTO = await _customerService.OpenAccountAsync(accountRequestDTO, cts.Token);

            accountDTO.Balance.Should().Be(0);
        }

        [Fact]
        public async Task AccountCreate_Unsuccessful()
        {
            // Arrange
            CancellationTokenSource cts = new();
            Customer customer = new(
                Guid.NewGuid(),
                "Harlem",
                "Williams",
                "harwill97@gmail.com",
                "111-111-1111",
                DateTimeOffset.Parse("10/20/1997"),
                Guid.NewGuid()
            );

            customers.Add(customer);

            CreateAccountRequestDTO accountRequestDTO = new(customer.Id, Enums.AccountType.Checking, "USD");

            // Act
            AccountDTO accountDTO = await _customerService.OpenAccountAsync(accountRequestDTO, cts.Token);

            // Assert
            List<Account> accounts = customer.Accounts.ToList();

            accounts.Should().ContainEquivalentOf(accountDTO, options => options
                .Excluding(a => a.Balance)
            );
        }


    }
}