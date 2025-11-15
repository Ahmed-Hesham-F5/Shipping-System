using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using System.Security.Claims;
using System.Web;

namespace ShippingSystem.Repositories
{
    public class UserRepository(UserManager<ApplicationUser> userManager, IAuthService authService,
        AppDbContext context, IMapper mapper, IEmailService emailService) : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IAuthService _authService = authService;
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;

        public async Task<OperationResult> CreateUserAsync(ApplicationUser user, string Password)
        {
            try
            {
                if (await IsEmailExistAsync(user.Email!))
                    return OperationResult.Fail(409, "Email already registered");

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
                if (await _userManager.IsInRoleAsync(user, role.ToString()))
                    return OperationResult.Ok();

                var addingRoleResult =
                   await _userManager.AddToRoleAsync(user, role.ToString());

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

        public async Task<ValueOperationResult<AuthDTO>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status400BadRequest, "Email or password is incorrect!");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var emailResult = await SendEmailConfirmationLinkAsync(
                    new RequestEmailConfirmationDto { ConfirmEmailUrl = loginDto.ConfirmEmailUrl, Email = loginDto.Email });

                if (!emailResult.Success)
                    return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred, please try again later.");

                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status403Forbidden, "Check your email to confirm your account.");
            }

            if (user.AccountStatus == AccountStatusEnum.Banned)
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status403Forbidden, "Account is banned");

            if (user.MustChangePassword)
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status403Forbidden, "Change Password Required");

            return await GetUserTokensAsync(user);
        }

        public async Task<ValueOperationResult<AuthDTO>> GetUserTokensAsync(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role != null)
                userClaims.Add(new Claim("role", role));

            _authService.CreateJwtToken(user, userClaims, out string Token, out DateTime ExpiresOn);

            RefreshToken refreshToken;
            do
            {
                refreshToken = _authService.GenerateRefreshToken();

            } while (user.RefreshTokens!.Any(x => x.Token == refreshToken.Token));

            user.RefreshTokens!.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return ValueOperationResult<AuthDTO>.Ok(new AuthDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                IsAuthenticated = true,
                Role = role ?? string.Empty,
                Token = Token,
                ExpiresOn = ExpiresOn,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            });
        }

        public async Task<ValueOperationResult<AuthDTO>> RefreshTokenAsync(string token)
        {
            try
            {
                var user =
                _context.RefreshTokens.Include(rt => rt.User).SingleOrDefault(t => t.Token == token)?.User;

                if (user == null)
                    return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status400BadRequest, "Invalid token");

                var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

                if (!refreshToken!.IsActive)
                    return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status400BadRequest, "Inactive token");

                refreshToken.RevokedOn = DateTime.UtcNow;

                return await GetUserTokensAsync(user);
            }
            catch (Exception ex)
            {
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<OperationResult> RevokeRefreshTokenAsync(string token)
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

        public async Task<OperationResult> RevokeAccessTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "User not found");

            user.AccessTokenVersion++;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<string>> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ValueOperationResult<string>.Fail(StatusCodes.Status404NotFound, "User not found");

            var role = await _userManager.GetRolesAsync(user);

            if (role == null || role.Count == 0)
                return ValueOperationResult<string>.Fail(StatusCodes.Status404NotFound, "Role not found");

            return ValueOperationResult<string>.Ok(role.FirstOrDefault()!);
        }

        public async Task<ValueOperationResult<AddressDto?>> GetUserAddressAsync(string userEmail)
        {
            if (await _userManager.FindByEmailAsync(userEmail) == null)
                return ValueOperationResult<AddressDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            AddressDto? Address = await _context.UserAddresses
                .Where(a => a.User.Email == userEmail)
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (Address == null)
                return ValueOperationResult<AddressDto?>.Fail(StatusCodes.Status404NotFound, "User address not found.");

            return ValueOperationResult<AddressDto?>.Ok(Address);
        }

        public async Task<OperationResult> RequestForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto)
        {
            if (string.IsNullOrEmpty(requestForgetPasswordDto.Email))
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "Email is required");
            
            var user = await _userManager.FindByEmailAsync(requestForgetPasswordDto.Email);

            if (user == null)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "User not found");
            else if (user.MustChangePassword)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");
            else if (user.AccountStatus == AccountStatusEnum.Banned)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Account is banned");


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(token);

            // Send email with reset link
            var emailResult = await _emailService.SendAsync(requestForgetPasswordDto.Email, "Reset Password",
                _emailService.RequestResetPasswordBody(requestForgetPasswordDto.ResetPasswordUrl, requestForgetPasswordDto.Email, token));

            if (!emailResult.Success)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred, please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

                if (user == null)
                    return OperationResult.Fail(StatusCodes.Status404NotFound, "User not found");

                var decodeToken = HttpUtility.UrlDecode(resetPasswordDto.Token);

                var result = await _userManager.ResetPasswordAsync(user, decodeToken, resetPasswordDto.NewPassword);

                if (!result.Succeeded)
                    return OperationResult.Fail(StatusCodes.Status400BadRequest, "Failed to reset password");
            }
            catch
            {
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }

            return OperationResult.Ok();
        }

        public async Task<OperationResult> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || !await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status400BadRequest, "Email or password is incorrect!");

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> FirstLoginChangePasswordAsync(FirstLoginChangePasswordDto firstLoginChangePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(firstLoginChangePasswordDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, firstLoginChangePasswordDto.CurrentPassword))
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status400BadRequest, "Email or password is incorrect!");
            else if (!user.MustChangePassword)
                return ValueOperationResult<ForgetPasswordResponseDto>.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");
            else if (user.AccountStatus == AccountStatusEnum.Banned)
                return ValueOperationResult<ForgetPasswordResponseDto>.Fail(StatusCodes.Status403Forbidden, "Account is banned");

            var result = await _userManager.
                    ChangePasswordAsync(user, firstLoginChangePasswordDto.CurrentPassword, firstLoginChangePasswordDto.NewPassword);

            if (!result.Succeeded)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            user.MustChangePassword = false;
            await _userManager.UpdateAsync(user);

            return OperationResult.Ok();
        }

        public async Task<OperationResult> SendEmailConfirmationLinkAsync(RequestEmailConfirmationDto requestEmailConfirmationDto)
        {
            var user = await _userManager.FindByEmailAsync(requestEmailConfirmationDto.Email);

            if (user == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "User not found");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var endcodeToken = HttpUtility.UrlEncode(token);

            var result = await _emailService.SendAsync(user.Email!, "Email confirmation",
                _emailService.EmailConfirmationBody(requestEmailConfirmationDto.ConfirmEmailUrl, requestEmailConfirmationDto.Email, endcodeToken));

            if (!result.Success)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred, please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);

                if (user == null)
                    return OperationResult.Fail(StatusCodes.Status404NotFound, "User not found");

                var decodeToken = HttpUtility.UrlDecode(confirmEmailDto.Token);

                var result = await _userManager.ConfirmEmailAsync(user, decodeToken);

                if (!result.Succeeded)
                    return OperationResult.Fail(StatusCodes.Status400BadRequest, $"Failed to confirm email");
            }
            catch
            {
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }

            return OperationResult.Ok();
        }
    }
}
