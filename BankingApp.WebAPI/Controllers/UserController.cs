
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.IdentityEntities;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly ICustomerService _customerService;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, ICustomerService customerService)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
        }
        
        [HttpPost("register")]
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
                    CreateCustomerRequestDTO request = new(user.FirstName!, user.LastName!, user.Email, user.PhoneNumber, user.DateOfBirth, user.Id);

                    if (ModelState.IsValid)
                    {
                        CustomerDTO response = await _customerService.CreateCustomerAsync(request, cancellationToken);
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
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
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if(user is null) return Problem("email not found", statusCode: 404, title:"Login");

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if(result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return BadRequest(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
