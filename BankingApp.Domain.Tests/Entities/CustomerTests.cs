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
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ICustomerService _customerService;

        private readonly List<Customer> _fakeDatabase = [];

        public CustomerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _customerRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken ct) =>
                    _fakeDatabase.FirstOrDefault(c => c.Email == email));

            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid id, CancellationToken ct) =>
                    _fakeDatabase.FirstOrDefault(c => c.Id == id));

            _customerRepositoryMock
                .Setup(repo => repo.AddCustomerAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Callback<Customer, CancellationToken>((customer, ct) => _fakeDatabase.Add(customer))
                .Returns(Task.CompletedTask);

            _customerService = new CustomerService(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
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
