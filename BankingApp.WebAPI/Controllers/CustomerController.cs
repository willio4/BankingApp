using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankingApp.Domain.Entities;
using BankingApp.Application.Common.Mappings;
using BankingApp.Infrastructure.Migrations;
using Microsoft.Identity.Client.NativeInterop;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
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
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers(CancellationToken cancellationToken)
        {
            var customers = await _context.Customers
                .Include(c => c.Accounts
                .Where(a => a.AccountStatus != Domain.Enums.AccountStatus.Closed))
                .ThenInclude(a => a.LedgerEntries)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            List<CustomerDTO> customerDtos = customers
                .Select(c => c.ToDTO())
                .OrderBy(c => c.CustomerStatus)
                .ToList();

            return customerDtos;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(Guid customerId, CancellationToken cancellationToken)
        {
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cancellationToken);

            if(customer == null)
            {
                return Problem("Customer doesn't exist", statusCode: 404, title: "Customer Search");
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> AddCustomer([FromBody] CreateCustomerRequestDTO customerRequestDTO, CancellationToken cancellationToken)
        {
            CustomerDTO customerDTO = await _customerService.CreateCustomerAsync(customerRequestDTO, cancellationToken);

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
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(CreateCustomerRequestDTO updated, Guid id, CancellationToken cancellationToken)
        {
            CustomerDTO? old = await _customerService.GetCustomerByIdAsync(id, cancellationToken);
            if (old is null) return Problem("Customer does not exist");
            CustomerDTO? current = await _customerService.UpdateCustomer(old, updated, cancellationToken);

            return Ok(current);
        }

        [HttpPost("{customerId}")]
        public async Task<ActionResult<CustomerDTO>> OpenAccount(Guid customerId, [FromBody] CreateAccountRequestDTO accountRequest, CancellationToken cancellationToken)
        {
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cancellationToken);

            if (customer is null) return Problem("Unknown customer id", statusCode: 404, title: "Open customer account");

            AccountDTO account = await _customerService.OpenAccountAsync(accountRequest, cancellationToken);

            return CreatedAtAction(nameof(GetCustomerById), new { customerId = customerId }, account);
        }

        [HttpPatch("delete-user/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId, CancellationToken cancellationToken)
        {
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cancellationToken);

            if (customer is null) return Problem("Unknown customer", statusCode: 404, title: "Customer Deletion");


            while (customer.Accounts.Count > 0)
            {
                try
                {
                    AccountDTO currentAccount = customer.Accounts[customer.Accounts.Count - 1];
                    await _accountService.CloseAccount(currentAccount, cancellationToken);
                }
                catch (Exception err)
                {
                    return Content($"{err.Message}");
                }
            }

            customer = await _customerService.DeleteCustomer(customer, cancellationToken);

            return Ok(customer);
        }
    }
}
