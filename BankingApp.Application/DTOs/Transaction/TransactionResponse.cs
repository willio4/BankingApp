using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs.Transaction
{
    public record TransactionResponse(
        Guid Id,
        string Description,
        DateTimeOffset Timestamp,
        decimal Amount,
        string Currency
    );
}