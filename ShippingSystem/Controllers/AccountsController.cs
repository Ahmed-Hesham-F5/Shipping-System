using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.Helpers;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.LoginUserAsync(loginDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            CookieHelper.SetRefreshTokenInCookie(Response, result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            var userAddress = await _userRepository.GetUserAddressAsync(loginDTO.Email);
            result.Value.City = userAddress.Value?.City ?? string.Empty;
            result.Value.Governorate = userAddress.Value?.Governorate ?? string.Empty;

            ApiResponse<AuthDTO> response = new ApiResponse<AuthDTO>(
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

            ApiResponse<AuthDTO> response = new ApiResponse<AuthDTO>(
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

            var result = await _userRepository.RevokeTokenAsync(token);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(success: false, message: result.ErrorMessage));

            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }
    }
}
