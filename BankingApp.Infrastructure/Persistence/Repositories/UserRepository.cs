using System.Data.Common;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Infrastructure.IdentityEntities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task AddUserAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            _db.Users.Add(user);
            return Task.CompletedTask;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }

}