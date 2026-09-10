using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public record Account(
        Guid Id,
        string AccountNumber,
        Guid CustomerId,
        AccountType Type,
        decimal Balance,
        string Currency
    );

    public record CreateAccountRequestDto(
        Guid CustomerId,
        AccountType AccountType,
        string Currency
    );

}