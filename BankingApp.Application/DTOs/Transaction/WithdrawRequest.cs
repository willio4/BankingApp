using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs
{
    public record WithdrawRequest(
        string AccountNumber,
        decimal Amount,
        string Currency
    );
}