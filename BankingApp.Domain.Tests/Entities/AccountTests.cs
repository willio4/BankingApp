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
        private readonly List<Customer> customers = new List<Customer>();
        private readonly List<Account> accounts = new List<Account>();

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
                DateTime.Parse("10/20/1997")
            );

            customers.Add(customer);

            CreateAccountRequestDTO accountRequestDTO = new(customer.Id, Enums.AccountType.Checking, "USD");

            // Act
            AccountDTO accountDTO = await _customerService.OpenAccountAsync(accountRequestDTO, cts.Token);

            // Assert
            List<Account> accounts = customer.GetAccounts();

            accounts.Should().ContainEquivalentOf(accountDTO, options => options
                .Excluding(a => a.Balance)
            );
        }


        [Fact]
        public void AccountCreateSuccessful_CalculateBalance()
        {
            Guid id = Guid.NewGuid();
            // Given
            Customer customer1 = new(id, "Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            Account account1 = new("000278405127", customer1.Id, Domain.Enums.AccountType.Checking);
            Money deposit = new(100);
            LedgerEntry deposit100 = new(account1.Id, deposit, Enums.EntryType.Credit, DateTime.UtcNow);
            Money withdraw = new(30);
            LedgerEntry withdraw30 = new(account1.Id, withdraw, Enums.EntryType.Debit, DateTime.UtcNow);
            // When
            account1.AddLedgerEntry(deposit100);
            account1.AddLedgerEntry(withdraw30);
            decimal expected = (deposit - withdraw).Amount;
            // Then
            Assert.Equal(expected, account1.CalculateBalance());
        }


    }
}