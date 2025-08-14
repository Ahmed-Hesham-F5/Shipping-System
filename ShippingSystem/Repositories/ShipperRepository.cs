using Microsoft.AspNetCore.Identity;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository: IShipperRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShipperRepository(AppDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> adduser(RegisterDto registerDto)
        {
         

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
                       Console.WriteLine( error.Description);
                    }
                }

                await _userManager.AddToRoleAsync(user, "Shipper");

                var shipper = new Shipper
                {
                    CompanyName = registerDto.CompanyName,
                    CompanyLink = registerDto.CompanyLink,
                    TypeOfTheProduction = registerDto.TypeOfTheProduction,
                    ShipperId = user.Id,
                };

                shipper.Addresses.Add(new Address
                {
                    City = registerDto.City,
                    Street = registerDto.Street,
                    Country = registerDto.Country,
                    Details = registerDto.Details,
                    ShipperId = shipper.ShipperId
                });

                shipper.Phones.Add(new Phone
                {
                    PhoneNumber = registerDto.PhoneNumber,
                    ShipperId = shipper.ShipperId
                });

                _context.Shippers.Add(shipper);
                await _context.SaveChangesAsync();

            return true;
        }
    }
}
