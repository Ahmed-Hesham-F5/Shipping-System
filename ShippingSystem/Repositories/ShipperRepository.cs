using Microsoft.AspNetCore.Identity;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShipperRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> AddUser(RegisterDto registerDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName
                };

                var creatingUserResult = await _userManager.CreateAsync(user, registerDto.Password);

                if (!creatingUserResult.Succeeded)
                    return false;

                var addingRoleResult = await _userManager.AddToRoleAsync(user, "Shipper");

                if (!addingRoleResult.Succeeded)
                    return false;

                var shipper = new Shipper
                {
                    CompanyName = registerDto.CompanyName,
                    CompanyLink = registerDto.CompanyLink,
                    TypeOfProduction = registerDto.TypeOfProduction,
                    ShipperId = user.Id,
                };

                shipper?.Addresses?.Add(new ShipperAddress
                {
                    City = registerDto.City,
                    Street = registerDto.Street,
                    Country = registerDto.Country,
                    Details = registerDto.Details,
                    ShipperId = shipper.ShipperId
                });

                shipper?.Phones?.Add(new ShipperPhone
                {
                    PhoneNumber = registerDto.PhoneNumber,
                    ShipperId = shipper.ShipperId
                });

                _context.Shippers.Add(shipper!);
                var saveResult = await _context.SaveChangesAsync();
                
                if (saveResult <= 0)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
