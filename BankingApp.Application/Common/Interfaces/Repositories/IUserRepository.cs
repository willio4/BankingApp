using BankingApp.Infrastructure.IdentityEntities;

namespace BankingApp.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task AddUserAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}