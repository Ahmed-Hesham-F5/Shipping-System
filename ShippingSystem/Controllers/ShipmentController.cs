using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Authorize(Roles = "Shipper")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {

        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentController(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        [HttpPost("addShipment")]
        public async Task<IActionResult> AddShipment([FromBody] ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var result = await _shipmentRepository.AddShipment(userId, shipmentDto);

            if (!result.Success)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return StatusCode(StatusCodes.Status201Created, "Shipment added successfully");
        }

        [HttpGet("getShipments")]
        public async Task<IActionResult> GetAllShipments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var shipments = await _shipmentRepository.GetAllShipments(userId);

            return Ok(shipments);
        }

        [HttpGet("getShipmentById/{id}")]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var result = await _shipmentRepository.GetShipmentById(userId, id);

            if (!result.Success)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            ApiResponse<GetShipmentDetailsDto?> shipment = new ApiResponse<GetShipmentDetailsDto?>
            (
                data: result.Value!,
                message: null!,
                success: true
            );

            return Ok(shipment);
        }

        [HttpPut("updateShipment/{id}")]
        public async Task<IActionResult> UpdateShipment(int id, [FromBody] ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var result = await _shipmentRepository.UpdateShipment(userId, id, shipmentDto);

            if (!result.Success)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Shipment updated successfully.");
        }

        [HttpDelete("deleteShipment/{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var result = await _shipmentRepository.DeleteShipment(userId, id);

            if (!result.Success)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }
    }
}
