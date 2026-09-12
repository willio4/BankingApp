using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public record AccountDTO(
        Guid Id,
        string AccountNumber,
        Guid CustomerId,
        AccountType Type,
        decimal Balance,
        string Currency
    );

    public record CreateAccountRequestDTO(
        Guid CustomerId,
        AccountType AccountType,
        string Currency
    );

}