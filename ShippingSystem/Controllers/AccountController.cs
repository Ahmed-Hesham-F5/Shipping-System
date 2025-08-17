using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     
        private readonly IShipperRepository _shipperRepository;
        public AccountController(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _shipperRepository.IsEmailExist(registerDto.Email);
            
            if (existingUser)
            {
                return BadRequest(new { message = "Email already registered!" });
            }

            var addShipperResult = await _shipperRepository.AddShipper(registerDto);

            if (!addShipperResult)
            {
                return StatusCode(500, new { message = "User registration failed" });
            }

            return Ok(new { message = "User registered successfully" });
        }
    }
}
