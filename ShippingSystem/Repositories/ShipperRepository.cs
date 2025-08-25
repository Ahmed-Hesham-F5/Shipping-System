using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using System.Transactions;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public ShipperRepository(AppDbContext context, UserManager<ApplicationUser> userManager,
            IAuthService authService)
        {
            _context = context;
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<AuthResponse> AddShipperAsync(ShipperRegisterDto ShipperRegisterDto)
        {
            AuthResponse auth = new AuthResponse();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = ShipperRegisterDto.Email,
                    Email = ShipperRegisterDto.Email,
                    FirstName = ShipperRegisterDto.FirstName,
                    LastName = ShipperRegisterDto.LastName
                };

                var creatingUserResult =
                    await _userManager.CreateAsync(user, ShipperRegisterDto.Password);

                if (!creatingUserResult.Succeeded)
                    throw new Exception(creatingUserResult.Errors.FirstOrDefault()?.Description);

                var addingRoleResult =
                    await _userManager.AddToRoleAsync(user, RolesEnum.Shipper.ToString());

                if (!addingRoleResult.Succeeded)
                    throw new Exception(addingRoleResult.Errors.FirstOrDefault()?.Description);

                var shipper = new Shipper
                {
                    CompanyName = ShipperRegisterDto.CompanyName,
                    CompanyLink = ShipperRegisterDto.CompanyLink,
                    TypeOfProduction = ShipperRegisterDto.TypeOfProduction,
                    ShipperId = user.Id,
                };

                shipper?.Addresses?.Add(new ShipperAddress
                {
                    City = ShipperRegisterDto.City,
                    Street = ShipperRegisterDto.Street,
                    Country = ShipperRegisterDto.Country,
                    Details = ShipperRegisterDto.Details,
                    ShipperId = shipper.ShipperId
                });

                shipper?.Phones?.Add(new ShipperPhone
                {
                    PhoneNumber = ShipperRegisterDto.PhoneNumber,
                    ShipperId = shipper.ShipperId
                });

                _context.Shippers.Add(shipper!);
                var saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                    throw new Exception("Bad request");

                await transaction.CommitAsync();
                return await _authService.LoginAsync(new LoginDto
                {
                    Email = ShipperRegisterDto.Email,
                    Password = ShipperRegisterDto.Password
                });
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                auth.IsAuthenticated = false;
                auth.Message = e.Message;
                return auth;
            }
        }

        public async Task<bool> IsEmailExistAsync(string email) =>
            await _userManager.FindByEmailAsync(email) != null;
    }
}
