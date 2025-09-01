using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    //[Authorize(Roles = "Shipper")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {

        private readonly IShipmentRepository _shipmentRepository;
        private readonly AppDbContext _context;

        public ShipperController(IShipmentRepository shipmentRepository, AppDbContext context)
        {
            _shipmentRepository = shipmentRepository;
            _context = context;
        }

        [HttpPost("addShipment")]
        public async Task<IActionResult> AddShipment([FromBody] ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var refreshToken = Request.Cookies["refreshToken"];
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || !storedToken.IsActive)
                return Unauthorized("User not authenticated.");

            var userId = storedToken.UserId;  

            var result = await _shipmentRepository.AddShipment(userId, shipmentDto);

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add shipment.");

            return Ok("Shipment added successfully.");
        }

        [HttpGet("getShipments")]
        public async Task<IActionResult> GetShipments()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || !storedToken.IsActive)
                return Unauthorized("User not authenticated.");

            var userId = storedToken.UserId;

            var shipments = await _shipmentRepository.GetAllShipments(userId);

            return Ok(shipments);
        }
    }
}
