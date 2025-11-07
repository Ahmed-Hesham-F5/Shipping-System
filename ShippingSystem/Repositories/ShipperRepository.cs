using ShippingSystem.Data;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.ShipperDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository(AppDbContext context,
        IUserRepository userRepository
        ) : IShipperRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ValueOperationResult<AuthDTO>> CreateShipperAsync(CreateShipperDto shipperRegisterDTO)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = shipperRegisterDTO.Email,
                    Email = shipperRegisterDTO.Email,
                    FirstName = shipperRegisterDTO.FirstName,
                    LastName = shipperRegisterDTO.LastName,
                    Role = RolesEnum.Shipper,
                };

                user.Phones?.Add(new UserPhone
                {
                    PhoneNumber = shipperRegisterDTO.PhoneNumber,
                    User = user,
                    UserId = user.Id
                });

                user.Addresses?.Add(new UserAddress
                {
                    City = shipperRegisterDTO.Address.City,
                    Street = shipperRegisterDTO.Address.Street,
                    Governorate = shipperRegisterDTO.Address.Governorate,
                    Details = shipperRegisterDTO.Address.Details,
                    GoogleMapAddressLink = shipperRegisterDTO.Address.GoogleMapAddressLink,
                    User = user,
                    UserID = user.Id
                });

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
    }
}
