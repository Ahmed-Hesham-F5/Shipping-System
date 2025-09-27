using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IUserRepository _userRepository;

        public AccountsController(IShipperRepository shipperRepository, IUserRepository userRepository)
        {
            _shipperRepository = shipperRepository;
            _userRepository = userRepository;
        }

        [HttpPost("shipperRegistration")]
        public async Task<IActionResult> ShipperRegistration([FromBody] ShipperRegisterDTO shipperRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shipperRepository.AddShipperAsync(shipperRegisterDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            SetRefreshTokenInCookie(result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            var shipperAddress = await _shipperRepository.GetShipperAddressAsync(shipperRegisterDTO.Email);

            result.Value.City = shipperAddress.Value?.City ?? string.Empty;
            result.Value.Governorate = shipperAddress.Value?.Governorate ?? string.Empty;

            ApiResponse<AuthDTO> response = new ApiResponse<AuthDTO>(
                success: true,
                message: "User registered successfully!",
                data: result.Value
            );

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.LoginUserAsync(loginDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            SetRefreshTokenInCookie(result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            if (result.Value.Role == (RolesEnum.Shipper.ToString()))
            {
                var shipperAddress = await _shipperRepository.GetShipperAddressAsync(loginDTO.Email);
                result.Value.City = shipperAddress.Value?.City ?? string.Empty;
                result.Value.Governorate = shipperAddress.Value?.Governorate ?? string.Empty;
            }

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

            SetRefreshTokenInCookie(result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

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

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
