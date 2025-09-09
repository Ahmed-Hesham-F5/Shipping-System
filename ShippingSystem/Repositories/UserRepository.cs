using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using System.Security.Claims;

namespace ShippingSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, IAuthService authService,
            AppDbContext context)
        {
            _userManager = userManager;
            _authService = authService;
            _context = context;
        }

        public async Task<OperationResult> CreateUserAsync(ApplicationUser user, string Password)
        {
            bool res = await IsEmailExistAsync(user.Email!);
            if (res)
                return OperationResult.Fail(409, "Email already registered");
            try
            {
                var creatingUserResult =
                   await _userManager.CreateAsync(user, Password);

                if (!creatingUserResult.Succeeded)
                {
                    Console.WriteLine(creatingUserResult.Errors.FirstOrDefault()?.Description);
                    return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }

            return OperationResult.Ok();
        }

        public async Task<OperationResult> AddRoleAsync(ApplicationUser user, RolesEnum role)
        {
            try
            {
                var addingRoleResult =
                   await _userManager.AddToRoleAsync(user, RolesEnum.Shipper.ToString());

                if (!addingRoleResult.Succeeded)
                {
                    Console.WriteLine(addingRoleResult.Errors.FirstOrDefault()?.Description);
                    return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }

            return OperationResult.Ok();
        }

        public async Task<bool> IsEmailExistAsync(string email) =>
           await _userManager.FindByEmailAsync(email) != null;

        public async Task<ValueOperationResult<AuthDto>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return ValueOperationResult<AuthDto>.Fail(StatusCodes.Status400BadRequest, "Email or password is incorrect!");

            if (user.AccountStatus == AccountStatus.Banned)
                return ValueOperationResult<AuthDto>.Fail(StatusCodes.Status400BadRequest, "Account is banned");

            return await GetUserTokensAsync(user);
        }

        public async Task<ValueOperationResult<AuthDto>> GetUserTokensAsync(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                userClaims.Add(new Claim("roles", role));

            _authService.CreateJwtToken(user, userClaims, out string Token, out DateTime ExpiresOn);

            RefreshToken refreshToken;
            do
            {
                refreshToken = _authService.GenerateRefreshToken();

            } while (user.RefreshTokens!.Any(x => x.Token == refreshToken.Token));

            user.RefreshTokens!.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return ValueOperationResult<AuthDto>.Ok(new AuthDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                IsAuthenticated = true,
                Roles = roles.ToList(),
                Token = Token,
                ExpiresOn = ExpiresOn,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            });
        }

        public async Task<ValueOperationResult<AuthDto>> RefreshTokenAsync(string token)
        {
            try
            {
                var user =
                _context.RefreshTokens.Include(rt => rt.User).SingleOrDefault(t => t.Token == token)?.User;

                if (user == null)
                    return ValueOperationResult<AuthDto>.Fail(StatusCodes.Status400BadRequest, "Invalid token");

                var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

                if (!refreshToken!.IsActive)
                    return ValueOperationResult<AuthDto>.Fail(StatusCodes.Status400BadRequest, "Inactive token");

                refreshToken.RevokedOn = DateTime.UtcNow;

                return await GetUserTokensAsync(user);
            }
            catch (Exception ex)
            {
                return ValueOperationResult<AuthDto>.Fail(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<OperationResult> RevokeTokenAsync(string token)
        {
            var user = _context.RefreshTokens.Include(rt => rt.User).SingleOrDefault(t => t.Token == token)?.User;

            if (user == null)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "Invalid token");

            var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

            if (!refreshToken!.IsActive)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "Inactive token");

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return OperationResult.Ok();
        }
    }
}
