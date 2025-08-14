using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     
        private readonly IShipperRepository _shipperRepository;
        public AccountController( IShipperRepository shipperRepository)
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
            await _shipperRepository.adduser(registerDto);   
         
         

            return Ok(new { message = "User registered successfully" });
        }
    }
}
