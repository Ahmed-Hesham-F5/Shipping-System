using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Models;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "Shipper");

            var shipper = new Shipper
            {
                CompanyName = registerDto.CompanyName,
                CompanyLink = registerDto?.CompanyLink,
                TypeOfTheProduction = registerDto?.TypeOfTheProduction,
                ApplicationUserId = user.Id,
            };

            shipper.Addresses.Add(new Address
            {
                City = registerDto.City,
                Street = registerDto.Street,
                Country = registerDto.Country,
                Details = registerDto.Details,
                ShipperId = shipper.Id
            });

            shipper.Phones.Add(new Phone
            {
                PhoneNumber = registerDto.PhoneNumber,
                ShipperId = shipper.Id
            });

            _context.Shippers.Add(shipper);
            await _context.SaveChangesAsync();

            var currentUser = await _userManager.FindByIdAsync(user.Id);
            currentUser.ShipperId = shipper.Id;
            await _userManager.UpdateAsync(currentUser);

            return Ok(new { message = "User registered successfully" });
        }
    }
}
