using System.Net;
using System.Reflection.Metadata.Ecma335;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public AccountController(ApplicationDbContext context, ICustomerService customerService, IAccountService accountService)
        {
            _context = context;
            _customerService = customerService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            if (_context.Accounts == null) return NotFound();
            return await _context.Accounts
                .ToListAsync();
        }

        [HttpGet("customer/{customerId:Guid}")]
        public async Task<ActionResult<IReadOnlyList<AccountDTO>>> GetCustomerAccounts(Guid customerId)
        {
            CancellationTokenSource cts = new();
            CustomerDTO? customer = await _customerService.GetCustomerByIdAsync(customerId, cts.Token);

            if(customer is null) return Problem("Unknown customer id", statusCode: 404, title: "Account Retrieval");

            return Ok(customer.Accounts);
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid accountId)
        {
            CancellationTokenSource cts = new();
            AccountDTO? account = await _accountService.GetAccountByIdAsync(accountId, cts.Token);

            if(account is null) return Problem("Unknown account id", statusCode: 404, title: "Account Search");

            return Ok(account);
        }

        [HttpPatch("close-account/{accountId}")]
        public async Task<ActionResult<AccountDTO>> DeleteAccount(Guid accountId)
        {
            CancellationTokenSource cts = new();
            AccountDTO? account = await _accountService.GetAccountByIdAsync(accountId, cts.Token);

            if(account is null) return Problem("Unknown account", statusCode: 404, title: "Account Deletion");

            if(account.Balance != 0) return Problem("Account balance must be zero before closing", statusCode: 405, title: "Account Deletion");

            account = await _accountService.CloseAccount(account, cts.Token);

            return Ok(account);
        }

    }
}
