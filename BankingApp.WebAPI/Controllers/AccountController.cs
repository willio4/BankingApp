using System.Net;
using System.Reflection.Metadata.Ecma335;
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
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;

        public AccountController(ApplicationDbContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            if (_context.Accounts == null) return NotFound();
            return await _context.Accounts
                .ToListAsync();
        }

        [HttpGet("{customerId:Guid}")]
        public async Task<ActionResult<IReadOnlyList<AccountDTO>>> GetCustomerAccounts(Guid customerId)
        {
            CancellationTokenSource cts = new();
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cts.Token);

            if(customer is null) return Problem("Unknown customer id", statusCode: 404, title: "Account Retrieval");

            return Ok(customer.Accounts);
        }

    }
}
