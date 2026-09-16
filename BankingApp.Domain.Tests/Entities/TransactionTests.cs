using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Application.Services;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Exceptions;
using BankingApp.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace BankingApp.Domain.Tests.Entities
{
    public class TransactionTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;


        private readonly CustomerService _customerService;
        private readonly AccountService _accountService;
        private readonly TransactionService _transactionService;
        private readonly List<Customer> customers = [];
        private readonly List<Account> accounts = [];
        private readonly List<Transaction> transactions = [];
        private readonly List<LedgerEntry> ledgerEntries = [];

        public TransactionTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _transactionServiceMock = new Mock<ITransactionService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            // Customer Repository Mock Setup
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

            // Account Repository Mock Setup
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

            // Transaction Repository Mock Setup
            _transactionRepositoryMock
                .Setup(repo => repo.AddTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
                .Callback<Transaction, CancellationToken>((transaction, ct) => transactions.Add(transaction))
                .Returns(Task.CompletedTask);

            _transactionRepositoryMock
                .Setup(repo => repo.GetByAccountIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns((Guid id, CancellationToken ct) =>
                    Task.FromResult<IReadOnlyList<Transaction>>(
                        [.. transactions.Where(t => t.Entries.Any(e => e.AccountId == id))]
                    )
                );

            _transactionRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid id, CancellationToken ct) =>
                    transactions.FirstOrDefault(t => t.Id == id));


            _accountService = new AccountService(
                _accountRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _unitOfWorkMock.Object
            );

            _customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _accountService
            );

            _transactionService = new TransactionService(
                _transactionRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _unitOfWorkMock.Object
            );


        }


        [Fact]
        public async Task SuccessfulTransaction()
        {
            CancellationTokenSource cts = new();
            // create 2 customers request
            CreateCustomerRequestDTO createCustomerRequestDTO = new(Guid.NewGuid(), "Harlem", "Williams", "harwill22@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));

            CreateCustomerRequestDTO createCustomerRequestDTO1 = new(Guid.NewGuid(), "Jersey", "Rowlette", "jerzrow21@gmail.com", "222-222-2222", DateTime.Parse("09/30/1995"));

            // create 2 customers
            CustomerDTO harlem = await _customerService.CreateCustomerAsync(createCustomerRequestDTO, cts.Token);
            CustomerDTO jersey = await _customerService.CreateCustomerAsync(createCustomerRequestDTO1, cts.Token);

            // create 2 account request
            CreateAccountRequestDTO accountRequestDTO = new(harlem.Id, Enums.AccountType.Checking, "USD");
            CreateAccountRequestDTO accountRequestDTO1 = new(jersey.Id, Enums.AccountType.Savings, "USD");

            AccountDTO? harlemAccount = await _customerService.OpenAccountAsync(accountRequestDTO, cts.Token);
            AccountDTO? jerzAccount = await _customerService.OpenAccountAsync(accountRequestDTO1, cts.Token);

            // create 2 deposit money request
            DepositMoneyRequestDTO depositMoneyRequestDTO = new(harlemAccount.AccountNumber, 5000m, "USD");
            DepositMoneyRequestDTO depositMoneyRequestDTO1 = new(jerzAccount.AccountNumber, 2300, "USD");

            await _transactionService.DepositMoneyAsync(depositMoneyRequestDTO, cts.Token);
            await _transactionService.DepositMoneyAsync(depositMoneyRequestDTO1, cts.Token);

            harlemAccount = await _accountService.GetAccountByIdAsync(harlemAccount.Id, cts.Token);
            jerzAccount = await _accountService.GetAccountByIdAsync(jerzAccount.Id, cts.Token);

            harlemAccount.Should().NotBeNull();
            harlemAccount.Balance.Should().Be(5000m);
            jerzAccount.Should().NotBeNull();
            jerzAccount.Balance.Should().Be(2300m);
        }

        [Fact]
        public void UnsuccessfulTransaction_MismatchCurrency()
        {
            Guid id = Guid.NewGuid();
            // build first account with $100
            Customer customer1 = new(id, "Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));
            Customer customer2 = new(id, "Jersey", "Rowlette", "jerzrowlette@gmail.com", "222-222-2222", DateTime.Parse("09/30/1995"));

            Account account1 = new("000278405127", customer1.Id, Domain.Enums.AccountType.Checking);
            Account account2 = new("010470247583", customer2.Id, Domain.Enums.AccountType.Checking, "EUR");
            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.UtcNow);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)



            Assert.Throws<CurrencyMismatchException>(() =>
            {
                // create transaction from account1 to account2 of $25.75
                Transaction.CreateTransfer(account1, account2, new(25.75m), "Tranferring $25.75 from account1 to account2");
            });
        }

        [Fact]
        public void UnsuccessfulTransaction_InsufficientFunds()
        {
            Guid id = Guid.NewGuid();
            // build first account with $100
            Customer customer1 = new(id, "Harlem", "Williams", "harwill2021@gmail.com", "111-111-1111", DateTime.Parse("10/20/1997"));
            Customer customer2 = new(id, "Jersey", "Rowlette", "jerzrowlette@gmail.com", "222-222-2222", DateTime.Parse("09/30/1995"));

            Account account1 = new("000278405127", customer1.Id, Domain.Enums.AccountType.Checking);
            Account account2 = new("010470247583", customer2.Id, Domain.Enums.AccountType.Checking);

            LedgerEntry deposit100 = new(account1.Id, new(100.00m), Enums.EntryType.Credit, DateTime.UtcNow);
            account1.AddLedgerEntry(deposit100);

            // build second account with $0 (no ledger entry)

            Assert.Throws<InsufficientFundsException>(() =>
            {
                // create transaction from account1 to account2 of $125.75
                Transaction.CreateTransfer(account1, account2, new(125.75m), "Tranferring $125.75 from account1 to account2");
            });
        }
    }
}