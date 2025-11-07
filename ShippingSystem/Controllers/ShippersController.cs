using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.ShipperDTOs;
using ShippingSystem.Helpers;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController(IShipperRepository shipperRepository,
        IUserRepository userRepository) : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository = shipperRepository;
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost]
        public async Task<IActionResult> CreateShipper([FromBody] CreateShipperDto shipperRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shipperRepository.CreateShipperAsync(shipperRegisterDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            CookieHelper.SetRefreshTokenInCookie(Response, result.Value?.RefreshToken!, result.Value!.RefreshTokenExpiration);

            var userEmail = result.Value.Email;

            if (userEmail == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(false, "An unexpected error occurred. Please try again later."));

            var userAddress = await _userRepository.GetUserAddressAsync(userEmail);
            result.Value.City = userAddress.Value?.City ?? string.Empty;
            result.Value.Governorate = userAddress.Value?.Governorate ?? string.Empty;

            ApiResponse<AuthDTO> response = new(
                success: true,
                message: "User registered successfully!",
                data: result.Value
            );

            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}
