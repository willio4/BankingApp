using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs.Transaction
{
    public record DepositRequest(
        string AccountNumber,
        decimal Amount,
        string Currency
    );
}