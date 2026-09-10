using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;
using BankingApp.Domain.ValueObjects;

namespace BankingApp.Application.DTOs
{
    public record TransferRequestDto(
        Guid SourceAccountId,
        Guid DestinationAccountId,
        decimal Amount,
        string Currency,
        string Description
    );
}