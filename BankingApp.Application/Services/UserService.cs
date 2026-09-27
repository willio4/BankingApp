

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
            ApplicationUser user = new(request.FirstName, request.LastName, request.DateOfBirth);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // if user created, use user to create customer
                CustomerRequest customerRequest = new(user.FirstName!, user.LastName!, user.Email!, user.PhoneNumber!, user.DateOfBirth, user.Id);

                CustomerResponse response = await _customerService.CreateCustomerAsync(customerRequest, cancellationToken);

                await _userRepository.AddUserAsync(user, cancellationToken);

                if (request.UserType == UserType.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserType.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new() { Name = UserType.Admin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, UserType.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserType.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new() { Name = UserType.User.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, UserType.User.ToString());
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return user.ToUserResponse();
            }
            else
            {
                return null!;
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
    }
}