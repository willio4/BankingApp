using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs
{
    public record TransactionDto(
        Guid Id,
        string Description,
        DateTime Timestamp,
        decimal Amount,
        string Currency
    );
}