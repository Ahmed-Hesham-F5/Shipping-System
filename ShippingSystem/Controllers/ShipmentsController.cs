using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.ShipmentDTOs;
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

            ApiResponse<List<ShipmentListDto>> response = new(
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

            ApiResponse<ShipmentDetailsDto?> response = new(
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

            ApiResponse<ShipmentDetailsDto?> response = new(
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

        [HttpGet("to-pickup")]
        public async Task<IActionResult> GetShipmentsReadyForPickup()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetShipmentsToPickup(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<ToPickupShipmentListDto>> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpGet("getShipmentsToReturn")]
        public async Task<IActionResult> GetShipmentsToReturn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetShipmentsToReturn(userId);
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<ToReturnShipmentListDto>> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpGet("get-shipment-status-statistics")]
        public async Task<IActionResult> GetShipmentStatusStatistics()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetShipmentStatusStatistics(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<ShipmentStatusStatisticsDto> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpPut("make-shipment-delivered/{shipmentId}")]
        public async Task<IActionResult> MakeShipmentDelivered(int shipmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.UpdateShipmentStatus(userId, shipmentId,
                Enums.ShipmentStatusEnum.Delivered,
                "Changed manually until we build the courier entity");

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return NoContent();
        }

        [HttpGet("to-cancel")]
        public async Task<IActionResult> GetShipmentsToCancel()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _shipmentRepository.GetShipmentsToCancel(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<ToCancelShipmentListDto>> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }
    }
}