using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using ShippingSystem.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ShippingSystem.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager,IAuthService authService,
            AppDbContext context ) {
            _userManager = userManager;
            _authService = authService;
            _context = context;
        }

        public async Task<OperationResult> CreateUserAsync(ApplicationUser user,string Password)
        {
            bool res = await IsEmailExistAsync(user.Email);
            if (res)
                return OperationResult.Fail("Email already registered");
            try
            {
                var creatingUserResult =
                   await _userManager.CreateAsync(user, Password);

                if (!creatingUserResult.Succeeded)
                {
                    ///////////
                    Console.WriteLine(creatingUserResult.Errors.FirstOrDefault()?.Description);
                    return OperationResult.Fail("Bad request");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return OperationResult.Fail("Bad request");
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
                    return OperationResult.Fail("Bad request");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return OperationResult.Fail("Bad request");
            }

            return OperationResult.Ok();

        }


        public async Task<bool> IsEmailExistAsync(string email) =>
           await _userManager.FindByEmailAsync(email) != null;

        public async Task<AuthResponse> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new AuthResponse { Message = "Email or password is incorrect!" };
            if(user.AccountStatus == AccountStatus.Banned)
                return new AuthResponse { Message = "Account is banned" };

            return await GetUserTokensAsync(user);    
        }

        public async Task<AuthResponse> GetUserTokensAsync(ApplicationUser user)
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

            return new AuthResponse
            {
                UserName = user.UserName!,
                Email = user.Email!,
                IsAuthenticated = true,
                Message = "User logged in successfully!",
                Roles = roles.ToList(),
                Token = Token,
                ExpiresOn = ExpiresOn,
                RefreshToken=refreshToken.Token,
                RefreshTokenExpiration=refreshToken.ExpiresOn
            };
        }
        public async Task<AuthResponse> RefreshTokenAsync(string token)
        {
            try
            {


                var AuthResponse = new AuthResponse();
                var user =
                _context.RefreshTokens.Include(rt => rt.User).SingleOrDefault(t => t.Token == token)?.User;
                if (user == null)
                {
                    AuthResponse.Message = "Invalid token";
                    return AuthResponse;
                }

                var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

                if (!refreshToken.IsActive)
                {
                    AuthResponse.Message = "Inactive token";
                    return AuthResponse;
                }

                refreshToken.RevokedOn = DateTime.UtcNow;

                return await GetUserTokensAsync(user);
            }
            catch (Exception ex)
            {
               return new AuthResponse{Message= ex.Message };
            }
            
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = _context.RefreshTokens.Include(rt=>rt.User).SingleOrDefault(t => t.Token == token)?.User;

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }






    }
}
