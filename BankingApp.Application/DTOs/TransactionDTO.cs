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
        DateTime Timestamp,
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
}