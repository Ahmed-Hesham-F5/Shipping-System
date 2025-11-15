using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.ShipperDTOs;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController(IShipperRepository shipperRepository) : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository = shipperRepository;

        [HttpPost]
        public async Task<IActionResult> CreateShipper([FromBody] CreateShipperDto shipperRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shipperRepository.CreateShipperAsync(shipperRegisterDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<string> response = new(
                success: true,
                message: "Check your email for the confirmation link!"
            );

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpGet("shipper-profile")]
        public async Task<IActionResult> GetShipperProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse<string>(false, "Unauthorized access."));

            var result = await _shipperRepository.GetShipperProfileAsync(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<ShipperProfileDto> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }
    }
}
