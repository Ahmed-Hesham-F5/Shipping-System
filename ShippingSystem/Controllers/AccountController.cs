using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Repositories;
using ShippingSystem.Services;

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
        public async Task<IActionResult> ShipperRegistration([FromBody] ShipperRegisterDto shipperRegisterDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var result = await _shipperRepository.AddShipperAsync(shipperRegisterDto);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            SetRefreshTokenInCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.LoginUserAsync(loginDto);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);


            return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _userRepository.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }



        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            var token = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _userRepository.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

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
