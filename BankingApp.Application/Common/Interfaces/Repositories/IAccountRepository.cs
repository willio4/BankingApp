using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.Common.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAccountAsync(Account account, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAccountAsync(Account account, CancellationToken cancellationToken = default);
    }
}