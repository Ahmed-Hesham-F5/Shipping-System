using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.Helpers;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var result = await _userRepository.LoginUserAsync(loginDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            CookieHelper.SetRefreshTokenInCookie(Response, result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            var userAddress = await _userRepository.GetUserAddressAsync(loginDto.Email);
            result.Value.City = userAddress.Value?.City ?? string.Empty;
            result.Value.Governorate = userAddress.Value?.Governorate ?? string.Empty;

            ApiResponse<AuthDTO> response = new(
                success: true,
                message: "User logged in successfully!",
                data: result.Value
            );

            return Ok(response);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ApiResponse<string>(false, "Token is required!"));

            var result = await _userRepository.RefreshTokenAsync(refreshToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(success: false, message: result.ErrorMessage));

            CookieHelper.SetRefreshTokenInCookie(Response, result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            ApiResponse<AuthDTO> response = new(
                success: true,
                message: "Token refreshed successfully!",
                data: result.Value
            );

            return Ok(response);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            var token = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ApiResponse<string>(false, "Token is required!"));

            var revokeRefreshTokenResult = await _userRepository.RevokeRefreshTokenAsync(token);

            if (!revokeRefreshTokenResult.Success)
                return StatusCode(revokeRefreshTokenResult.StatusCode,
                    new ApiResponse<string>(success: false, message: revokeRefreshTokenResult.ErrorMessage));

            Response.Cookies.Delete("refreshToken");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var revokeAccessTokenResult = await _userRepository.RevokeAccessTokenAsync(userId);

            if (!revokeAccessTokenResult.Success)
                return StatusCode(revokeAccessTokenResult.StatusCode,
                    new ApiResponse<string>(success: false, message: revokeAccessTokenResult.ErrorMessage));

            return NoContent();
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] RequestForgetPasswordDto requestForgetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.RequestForgetPasswordAsync(requestForgetPasswordDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<string> response = new(
                success: true,
                message: "Check your email for password reset link!"
            );

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.ResetPasswordAsync(resetPasswordDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<string>(true, "Password reset successfully!"));
        }

        [HttpPost("first-login-change-password")]
        public async Task<IActionResult> FirstLoginChangePassword([FromBody] FirstLoginChangePasswordDto firstLoginChangePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.FirstLoginChangePasswordAsync(firstLoginChangePasswordDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<string>(true, "Password changed successfully!"));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _userRepository.ChangePasswordAsync(userId, changePasswordDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<string>(true, "Password changed successfully!"));
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailDto confirmEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.ConfirmEmailAsync(confirmEmailDto);
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<string>(true, "Email confirmed successfully!"));
        }

        [HttpGet("resend-email-confirmation-link")]
        public async Task<IActionResult> ResendEmailConfirmationLink([FromQuery] RequestEmailConfirmationDto requestEmailConfirmationDto)
        {
            if (string.IsNullOrEmpty(requestEmailConfirmationDto.Email))
                return BadRequest(new ApiResponse<string>(false, "Email is required."));

            var result = await _userRepository.SendEmailConfirmationLinkAsync(requestEmailConfirmationDto);
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<string>(true, "Email confirmation link resent successfully!"));
        }
    }
}