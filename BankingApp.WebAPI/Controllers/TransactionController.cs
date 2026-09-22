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
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(Guid transactionId, CancellationToken cancellationToken)
        {
            TransactionDTO? transaction = await _transactionService.GetTransactionByIdAsync(transactionId, cancellationToken);

            if(transaction is null) return Problem("Unknown transaction id", statusCode: 404, title: "Transaction Search");

            return Ok(transaction);
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<TransactionDTO>> Deposit([FromBody]DepositMoneyRequestDTO requestDTO, CancellationToken cancellationToken)
        {
            TransactionDTO transaction = await _transactionService.DepositMoneyAsync(requestDTO, cancellationToken);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.Id }, transaction);
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<TransactionDTO>> Withdraw([FromBody] WithdrawMoneyRequestDTO requestDTO, CancellationToken cancellationToken)
        {
            TransactionDTO transaction = await _transactionService.WithdrawMoneyAsync(requestDTO, cancellationToken);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.Id }, transaction);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransactionDTO>> Transfer([FromBody] CreateTransactionRequestDTO requestDTO, CancellationToken cancellationToken)
        {
            TransactionDTO transaction = await _transactionService.TransferMoneyAsync(requestDTO, cancellationToken);
            return CreatedAtAction(nameof(GetTransactionById), new {transactionId = transaction.Id}, transaction);
        }

        [HttpGet("history/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAccountTransactionHistory(Guid accountId, CancellationToken cancellationToken)
        {
            IReadOnlyList<TransactionDTO> transactions = await _transactionService.GetAccountTransactionHistoryAsync(accountId, cancellationToken);

            return Ok(transactions);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions(CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Entries)
                .ToListAsync(cancellationToken);
            return Ok(transactions);
        }

    }
}
