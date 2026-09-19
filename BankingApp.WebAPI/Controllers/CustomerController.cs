using System.Net;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;

        public CustomerController(ApplicationDbContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if(_context.Customers == null) return NotFound();
            return await _context.Customers.ToListAsync();
        }

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(Guid customerId)
        {
            if(_context.Customers == null) return NotFound();

            Customer? customer = await _context.Customers.FindAsync(customerId);

            CancellationTokenSource cts = new();

            if(customer == null)
            {
                return Problem("Customer doesn't exist", statusCode: 404, title: "Customer Search");
            }

            CustomerDTO? customerDTO = await _customerService.GetCustomerByIdAsync(customer.Id, cts.Token);
        
            if(customerDTO is null) {
                return Problem("Customer doesn't exist", statusCode: 404, title: "Customer Search");
            }

            return customerDTO;
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
    }
}
