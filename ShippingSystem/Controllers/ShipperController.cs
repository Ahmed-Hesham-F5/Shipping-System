using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Authorize(Roles = "Shipper")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {

        private readonly IShipmentRepository _shipmentRepository;

        public ShipperController(IShipmentRepository shipmentRepository)
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

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add shipment.");

            return Ok("Shipment added successfully.");
        }

        [HttpGet("getShipments")]
        public async Task<IActionResult> GetShipments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var shipments = await _shipmentRepository.GetAllShipments(userId);

            return Ok(shipments);
        }
    }
}
