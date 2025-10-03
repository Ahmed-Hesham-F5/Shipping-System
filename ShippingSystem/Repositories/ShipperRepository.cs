using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public ShipperRepository(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<ValueOperationResult<AuthDTO>> AddShipperAsync(ShipperRegisterDTO shipperRegisterDTO)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = shipperRegisterDTO.Email,
                    Email = shipperRegisterDTO.Email,
                    FirstName = shipperRegisterDTO.FirstName,
                    LastName = shipperRegisterDTO.LastName
                };

                var CreateUserResult = await _userRepository.CreateUserAsync(user, shipperRegisterDTO.Password);

                if (!CreateUserResult.Success)
                    return ValueOperationResult<AuthDTO>.Fail(CreateUserResult.StatusCode, CreateUserResult.ErrorMessage);

                var addShipperRoleResult = await _userRepository.AddRoleAsync(user, RolesEnum.Shipper);

                if (!addShipperRoleResult.Success)
                    return ValueOperationResult<AuthDTO>.Fail(addShipperRoleResult.StatusCode, addShipperRoleResult.ErrorMessage);

                var shipper = new Shipper
                {
                    CompanyName = shipperRegisterDTO.CompanyName,
                    CompanyLink = shipperRegisterDTO.CompanyLink,
                    TypeOfProduction = shipperRegisterDTO.TypeOfProduction,
                    ShipperId = user.Id,
                };

                shipper?.Addresses?.Add(new ShipperAddress
                {
                    City = shipperRegisterDTO.City,
                    Street = shipperRegisterDTO.Street,
                    Governorate = shipperRegisterDTO.Governorate,
                    Details = shipperRegisterDTO.AddressDetails,
                    ShipperId = shipper.ShipperId
                });

                shipper?.Phones?.Add(new ShipperPhone
                {
                    PhoneNumber = shipperRegisterDTO.PhoneNumber,
                    ShipperId = shipper.ShipperId
                });

                _context.Shippers.Add(shipper!);
                var saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                    return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

                await transaction.CommitAsync();

                return await _userRepository.GetUserTokensAsync(user);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e.Message.ToString());
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ValueOperationResult<ShipperAddressDto?>> GetShipperAddressAsync(string shipperUserEmail)
        {
            if (string.IsNullOrEmpty(shipperUserEmail))
                return ValueOperationResult<ShipperAddressDto?>.Fail(StatusCodes.Status400BadRequest, "Invalid shipper username.");

            var shipper = await _context.Shippers
                .Include(s => s.ApplicationUser)
                .Include(s => s.Addresses)
                .Where(s => s.ApplicationUser.Email == shipperUserEmail)
                .Select(s => new
                {
                    Addresses = s.Addresses!.Select(address => new ShipperAddressDto
                    {
                        City = address.City,
                        Street = address.Street,
                        Governorate = address.Governorate,
                        Details = address.Details
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (shipper == null)
                return ValueOperationResult<ShipperAddressDto?>.Fail(StatusCodes.Status404NotFound, "Shipper address not found.");

            return ValueOperationResult<ShipperAddressDto?>.Ok(shipper.Addresses.FirstOrDefault());
        }
    }
}
