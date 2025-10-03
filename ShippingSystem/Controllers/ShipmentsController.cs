using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Authorize(Roles = "Shipper")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {

        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentsController(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        [HttpPost("addShipment")]
        public async Task<IActionResult> AddShipment([FromBody] CreateShipmentDto shipmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.AddShipment(userId, shipmentDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<string>(true, "Shipment added successfully"));
        }

        [HttpGet("getShipments")]
        public async Task<IActionResult> GetAllShipments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetAllShipments(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<ShipmentListDto>> response = new ApiResponse<List<ShipmentListDto>>
            (
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpGet("getShipmentById/{id}")]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetShipmentById(userId, id);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<ShipmentDetailsDto?> response = new ApiResponse<ShipmentDetailsDto?>
            (
                data: result.Value!,
                message: null!,
                success: true
            );

            return Ok(response);
        }

        [HttpPut("updateShipment/{id}")]
        public async Task<IActionResult> UpdateShipment(int id, [FromBody] UpdateShipmentDto shipmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.UpdateShipment(userId, id, shipmentDTO);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<ShipmentDetailsDto?> response = new ApiResponse<ShipmentDetailsDto?>
            (
                data: result.Value!,
                message: null!,
                success: true
            );

            return Ok(response);
        }

        [HttpDelete("deleteShipment/{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.DeleteShipment(userId, id);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return NoContent();
        }

        [HttpGet("getPendingShipments")]
        public async Task<IActionResult> GetPendingShipments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetAllPendingShipments(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<PendingShipmentListDto>> response = new ApiResponse<List<PendingShipmentListDto>>
            (
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpPost("pickupRequest")]
        public async Task<IActionResult> pickupRequest([FromBody] CreatePickupRequestDto pickupRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.CreatePickupRequest(userId, pickupRequestDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<string>(true, "Pickup request created successfully."));
        }

        [HttpGet("getAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetAllRequests(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<RequestListDto>> response = new ApiResponse<List<RequestListDto>>
            (
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }
    }
}
