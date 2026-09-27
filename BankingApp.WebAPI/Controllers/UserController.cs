
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.IdentityEntities;
using BankingApp.Infrastructure.Persistence;
using BankingApp.Infrastructure.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly ICustomerService _customerService;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext db, ICustomerService customerService)
        {
            _db = db;
            _userManager = userManager;
            _customerService = customerService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Application.DTOs.RegisterRequest u, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(u);
            }

            Guid id = Guid.NewGuid();
            ApplicationUser user = new(u.FirstName, u.LastName, u.DateOfBirth)
            {
                UserName = u.Email,
                Email = u.Email,
                Id = id,
                PhoneNumber = u.PhoneNumber,
            };

            await _db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, u.Password);

                if (result.Succeeded)
                {
                    // if user created, use user to create customer
                    CustomerRequest request = new(user.FirstName!, user.LastName!, user.Email, user.PhoneNumber, user.DateOfBirth, user.Id);

                    CustomerResponse response = await _customerService.CreateCustomerAsync(request, cancellationToken);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (System.Exception ex)
            {
                // if exception is thrown then rollback
                await _db.Database.RollbackTransactionAsync(cancellationToken);
                return BadRequest(ex.InnerException?.Message);
            }

            await _db.Database.CommitTransactionAsync(cancellationToken);

            return Ok(u.ToResponse());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, ITokenProvider tokenProvider, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);



            if (user is null || await _userManager.CheckPasswordAsync(user, request.Password) == false) return Unauthorized();

            var (accessToken, refreshToken) = await tokenProvider.GenerateTokens(user);

            return Ok(new LoginResponse(accessToken, refreshToken));

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest refreshToken)
        {
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest, ITokenProvider tokenProvider, IRefreshTokenService _refreshTokenService, CancellationToken cancellationToken)
        {
            RefreshToken? token = _db.RefreshTokens.FirstOrDefault(t => t.Token == refreshTokenRequest.RefreshToken);

            if (token is null || DateTime.UtcNow >= token.ExpiresAt || token.IsDenied) return Unauthorized();

            ApplicationUser? user = await _userManager.FindByIdAsync(token.UserId.ToString());

            if (user is null) return Unauthorized();

            var at = tokenProvider.GenerateAccessToken(user);

            return Ok(new RefreshTokenResponse(at, token.Token!));
        }
    }
}
