using Azure.Core;
using BankingApp.Application.Common.Interfaces.Repositories;
using BankingApp.Application.Common.Interfaces.Services;
using BankingApp.Application.DTOs;
using BankingApp.Application.DTOs.Login;
using BankingApp.Application.DTOs.RefreshToken;
using BankingApp.Application.DTOs.User;
using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.IdentityEntities;
using BankingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class AuthController(IUnitOfWork unitOfWork, IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRequest u, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            UserResponse? user = await _userService.CreateUserAndCustomerAsync(u, cancellationToken);

            if (user is null) return BadRequest();

            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, ITokenService tokenService, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userService.TryLogin(request, cancellationToken))
            {
                // if login is successful, generate tokens
                string accessToken = await tokenService.GenerateAccessToken(request, cancellationToken);
                string refreshToken = await tokenService.GenerateRefreshToken(request, cancellationToken);

                LoginResponse response = new(accessToken, refreshToken);
                return Ok(response);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest, ITokenService _tokenService, CancellationToken cancellationToken)
        {

            RefreshTokenResponse? refreshToken = await _tokenService.GetRefreshTokenAsync(refreshTokenRequest, cancellationToken);

            if (refreshToken is not null)
            {
                string accessToken = await _tokenService.RefreshAccessToken(refreshTokenRequest, cancellationToken);
                return Ok(new { accessToken, refreshToken.RefreshToken });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest refreshToken, ITokenService tokenService, CancellationToken cancellationToken)
        {
            await tokenService.RevokeTokenAsync(refreshToken.RefreshToken, cancellationToken);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
