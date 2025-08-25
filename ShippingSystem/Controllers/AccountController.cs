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
        private readonly IAuthService _authService;

        public AccountController(IShipperRepository shipperRepository, IAuthService authService)
        {
            _shipperRepository = shipperRepository;
            _authService = authService;
        }

        [HttpPost("shipperRegistration")]
        public async Task<IActionResult> ShipperRegistration([FromBody] ShipperRegisterDto shipperRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shipperRepository.AddShipperAsync(shipperRegisterDto);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
