using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperAccountController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository;

        private readonly IAuthService _authService;

        public ShipperAccountController(IShipperRepository shipperRepository,IAuthService authService)
        {
            _shipperRepository = shipperRepository;
              _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ShipperRegisterDto ShipperRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          //  var result = await _authService.RegisterAsync(ShipperRegisterDto);
            var result = await _shipperRepository.AddShipperAsync(ShipperRegisterDto);

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
