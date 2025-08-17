using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {

        private readonly IShipmentRepository _shipmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShipmentsController(IShipmentRepository shipmentRepository, UserManager<ApplicationUser> userManager)
        {
            _shipmentRepository = shipmentRepository;
            _userManager = userManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddShipment(string userId, [FromBody] ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userManager.Users.Any(u => u.Id == userId))
                return Unauthorized("User not authenticated.");

            var result = await _shipmentRepository.AddShipment(userId, shipmentDto);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add shipment.");
            return Ok("Shipment added successfully.");
        }

        [HttpGet("GetShipments/{userId}")]
        public async Task<IActionResult> GetShipments(string userId)
        {
            if (!_userManager.Users.Any(u => u.Id == userId))
                return Unauthorized("User not authenticated.");

            var shipments = await _shipmentRepository.GetAllShipments(userId);
            return Ok(shipments);
        }
    }
}
