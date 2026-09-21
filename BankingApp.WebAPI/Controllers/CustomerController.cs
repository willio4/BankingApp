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
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            if(_context.Customers == null) return NotFound();

            CancellationTokenSource cts = new();

            var customers = await _context.Customers
                .Include(c => c.Accounts
                .Where(a => a.AccountStatus != Domain.Enums.AccountStatus.Closed))
                .ThenInclude(a => a.LedgerEntries)
                .AsNoTracking()
                .ToListAsync(cts.Token);

            List<CustomerDTO> customerDtos = customers
                .Select(c => c.ToDTO())
                .OrderBy(c => c.CustomerStatus)
                .ToList();

            return customerDtos;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(Guid customerId)
        {
            if(_context.Customers == null) return NotFound();

            CancellationTokenSource cts = new();

            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cts.Token);

            if(customer == null)
            {
                return Problem("Customer doesn't exist", statusCode: 404, title: "Customer Search");
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> AddCustomer([FromBody]CreateCustomerRequestDTO customerRequestDTO)
        {
            CancellationTokenSource cts = new();
            CustomerDTO customerDTO = await _customerService.CreateCustomerAsync(customerRequestDTO, cts.Token);

            if(ModelState.IsValid)
            {
                return CreatedAtAction(nameof(GetCustomerById), new { customerId = customerDTO.Id }, customerDTO);
            }
            else
            {

                return Problem("Customer could not be created");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(CreateCustomerRequestDTO updated, Guid id)
        {
            CancellationTokenSource cts = new();
            CustomerDTO? old = await _customerService.GetCustomerByIdAsync(id, cts.Token);
            if(old is null) return Problem("Customer does not exist");
            CustomerDTO? current = await _customerService.UpdateCustomer(old, updated, cts.Token);

            return Ok(current);
        }

        [HttpPost("{customerId}")]
        public async Task<ActionResult<CustomerDTO>> OpenAccount(Guid customerId, [FromBody] CreateAccountRequestDTO accountRequest)
        {
            CancellationTokenSource cts = new();
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cts.Token);

            if(customer is null) return Problem("Unknown customer id", statusCode: 404, title: "Open customer account");

            AccountDTO account = await _customerService.OpenAccountAsync(accountRequest, cts.Token);

            return CreatedAtAction(nameof(GetCustomerById), new {customerId = customerId}, account);
        }

        [HttpPatch("delete-user/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            CancellationTokenSource cts = new();
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cts.Token);

            if(customer is null) return Problem("Unknown customer", statusCode: 404, title: "Customer Deletion");


            while(customer.Accounts.Count > 0)
            {
                try
                {
                    AccountDTO currentAccount = customer.Accounts[customer.Accounts.Count - 1];
                    await _accountService.CloseAccount(currentAccount, cts.Token);
                } catch(Exception err)
                {
                    return Content( $"{err.Message}" );
                }
            } 

            customer = await _customerService.DeleteCustomer(customer, cts.Token);

            return Ok(customer);
        }
    }
}
