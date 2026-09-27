using BankingApp.Application.DTOs;
using BankingApp.Application.DTOs.Login;
using BankingApp.Application.DTOs.User;

namespace BankingApp.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        public Task<UserResponse?> CreateUserAndCustomerAsync(UserRequest request, CancellationToken cancellationToken = default);
        public Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> TryLogin(LoginRequest request, CancellationToken cancellationToken);
    }
}