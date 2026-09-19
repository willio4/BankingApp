using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<Customer>> GetCustomerById(Guid customerId)
        {
            if(_context.Customers == null) return NotFound();

            Customer? customer = await _context.Customers.FindAsync(customerId);

            if(customer == null)
            {
                return Problem("Customer doesn't exist", statusCode: 404, title: "Customer Search");
            }

            return customer;
        }
    }
}
