

using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.Common.Mappings;
using BankingApp.Application.DTOs;
using BankingApp.Application.DTOs.Customer;
using BankingApp.Application.DTOs.Login;
using BankingApp.Application.DTOs.User;
using BankingApp.Domain.Enums;
using BankingApp.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, ICustomerService customerService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<UserResponse?> CreateUserAndCustomerAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            ApplicationUser user = new(request.FirstName, request.LastName, request.DateOfBirth)
            {
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    return null; 
                }

                CustomerRequest customerRequest = new(
                    user.FirstName!,
                    user.LastName!,
                    user.Email!,
                    user.PhoneNumber!,
                    user.DateOfBirth,
                    user.Id
                );

                await _customerService.CreateCustomerAsync(customerRequest, cancellationToken);

                string roleName = request.UserType.ToString();
                await EnsureRoleExistsAndAssignAsync(user, roleName);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return user.ToUserResponse();
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
        public async Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userRepository.GetUserByEmailAsync(email, cancellationToken) ?? throw new InvalidOperationException();

            return user.ToUserResponse();
        }

        public async Task<bool> TryLogin(LoginRequest request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || await _userManager.CheckPasswordAsync(user, request.Password) == false) return false;

            return true;
        }

        private async Task EnsureRoleExistsAndAssignAsync(ApplicationUser user, string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) is null)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}