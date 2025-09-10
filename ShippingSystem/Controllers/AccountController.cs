using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(IShipperRepository shipperRepository, IUserRepository userRepository)
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
                    new ApiResponse<AuthDTO>(false, result.ErrorMessage));

            SetRefreshTokenInCookie(result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

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
                    new ApiResponse<AuthDTO>(false, result.ErrorMessage));

            SetRefreshTokenInCookie(result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Token is required!");

            var result = await _userRepository.RefreshTokenAsync(refreshToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<AuthDTO>(success: false, message: result.ErrorMessage));

            SetRefreshTokenInCookie(result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            var token = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _userRepository.RevokeTokenAsync(token);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<AuthDTO>(success: false, message: result.ErrorMessage));

            Response.Cookies.Delete("refreshToken");

            return Ok();
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
