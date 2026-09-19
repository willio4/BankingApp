using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs
{
    public record TransactionDTO(
        Guid Id,
        string Description,
        DateTimeOffset Timestamp,
        decimal Amount,
        string Currency
    );

    public record CreateTransactionRequestDTO(
        Guid SourceAccountId,
        Guid DestinationAccountId,
        decimal Amount,
        string Currency,
        string Description
    );

    public record DepositMoneyRequestDTO(
        string AccountNumber,
        decimal Amount,
        string Currency
    );

    public record WithdrawMoneyRequestDTO(
        string AccountNumber,
        decimal Amount,
        string Currency
    );
}