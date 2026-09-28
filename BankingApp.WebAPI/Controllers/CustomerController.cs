using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankingApp.Domain.Entities;
using BankingApp.Application.Common.Mappings;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Application.DTOs.Customer;
using BankingApp.Application.DTOs.Account;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(ApplicationDbContext context, ICustomerService customerService, IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _context = context;
            _customerService = customerService;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomers(CancellationToken cancellationToken)
        {
            var customers = await _context.Customers
                .Include(c => c.Accounts
                .Where(a => a.AccountStatus != Domain.Enums.AccountStatus.Closed))
                .ThenInclude(a => a.LedgerEntries)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            List<CustomerResponse> customerDtos = customers
                .Select(c => c.ToCustomerResponse())
                .OrderBy(c => c.CustomerStatus)
                .ToList();

            return customerDtos;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerById(Guid customerId, CancellationToken cancellationToken)
        {
            CustomerResponse? customer = await _customerService.GetCustomerByIdAsync(customerId, cancellationToken);

            if (customer == null)
            {
                return Problem("Customer doesn't exist", statusCode: 404, title: "Customer Search");
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> AddCustomer([FromBody] CustomerRequest customerRequestDTO, CancellationToken cancellationToken)
        {
            CustomerResponse customerDTO = await _customerService.CreateCustomerAsync(customerRequestDTO, cancellationToken);

            if (ModelState.IsValid)
            {
                return CreatedAtAction(nameof(GetCustomerById), new { customerId = customerDTO.Id }, customerDTO);
            }
            else
            {

                return Problem("Customer could not be created");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResponse>> UpdateCustomer(CustomerRequest updated, Guid id, CancellationToken cancellationToken)
        {
            CustomerResponse? old = await _customerService.GetCustomerByIdAsync(id, cancellationToken);
            if (old is null) return Problem("Customer does not exist");
            CustomerResponse? current = await _customerService.UpdateCustomerAsync(old, updated, cancellationToken);

            return Ok(current);
        }

        [HttpPost("open-account")]
        public async Task<ActionResult<CustomerResponse>> OpenAccount([FromBody] AccountRequest accountRequest, CancellationToken cancellationToken)
        {
            CustomerResponse? customer = await _customerService.GetCustomerByIdAsync(accountRequest.CustomerId, cancellationToken);

            if (customer is null) return Problem("Unknown customer id", statusCode: 404, title: "Open customer account");

            AccountResponse account = await _customerService.OpenAccountAsync(accountRequest, cancellationToken);

            return CreatedAtAction(nameof(GetCustomerById), new { customerId = accountRequest.CustomerId }, account);
        }

        [HttpPatch("delete-user/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId, CancellationToken cancellationToken)
        {
            CustomerResponse? customer = await _customerService.GetCustomerByIdAsync(customerId, cancellationToken);

            if (customer is null) return Problem("Unknown customer", statusCode: 404, title: "Customer Deletion");


            while (customer.Accounts.Count > 0)
            {
                try
                {
                    AccountResponse currentAccount = customer.Accounts[customer.Accounts.Count - 1];
                    await _accountService.CloseAccountAsync(currentAccount, cancellationToken);
                }
                catch (Exception err)
                {
                    return Content($"{err.Message}");
                }
            }

            customer = await _customerService.DeleteCustomerAsync(customer, cancellationToken);

            return Ok(customer);
        }
    }
}
