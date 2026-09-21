using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Domain.ValueObjects;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _context;
        public TransactionController(ITransactionService transactionService, ApplicationDbContext context)
        {
            _transactionService = transactionService;
            _context = context;
        }
        [HttpGet("{transactionId}")]
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(Guid transactionId)
        {
            CancellationTokenSource cts = new();
            TransactionDTO? transaction = await _transactionService.GetTransactionByIdAsync(transactionId, cts.Token);

            if(transaction is null) return Problem("Unknown transaction id", statusCode: 404, title: "Transaction Search");

            return Ok(transaction);
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<TransactionDTO>> Deposit([FromBody]DepositMoneyRequestDTO requestDTO)
        {
            CancellationTokenSource cts = new();
            TransactionDTO transaction = await _transactionService.DepositMoneyAsync(requestDTO, cts.Token);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.Id }, transaction);
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<TransactionDTO>> Withdraw([FromBody] WithdrawMoneyRequestDTO requestDTO)
        {
            CancellationTokenSource cts = new();
            TransactionDTO transaction = await _transactionService.WithdrawMoneyAsync(requestDTO, cts.Token);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.Id }, transaction);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransactionDTO>> Transfer([FromBody] CreateTransactionRequestDTO requestDTO)
        {
            CancellationTokenSource cts = new();
            TransactionDTO transaction = await _transactionService.TransferMoneyAsync(requestDTO, cts.Token);
            return CreatedAtAction(nameof(GetTransactionById), new {transactionId = transaction.Id}, transaction);
        }

        [HttpGet("history/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAccountTransactionHistory(Guid accountId)
        {
            CancellationTokenSource cts = new();
            IReadOnlyList<TransactionDTO> transactions = await _transactionService.GetAccountTransactionHistoryAsync(accountId, cts.Token);

            return Ok(transactions);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            CancellationTokenSource cts = new();
            var transactions = await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Entries)
                .ToListAsync(cts.Token);
            return Ok(transactions);
        }

    }
}
