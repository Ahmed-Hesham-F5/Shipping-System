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
        IUserRepository userRepository,
        IEmailService emailService
        ) : IShipperRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;

        public async Task<OperationResult> CreateShipperAsync(CreateShipperDto createShipperDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = createShipperDto.Email,
                    Email = createShipperDto.Email,
                    FirstName = createShipperDto.FirstName,
                    LastName = createShipperDto.LastName,
                    Role = RolesEnum.Shipper,
                };

                user.Phones?.Add(new UserPhone
                {
                    PhoneNumber = createShipperDto.PhoneNumber,
                    User = user,
                    UserId = user.Id
                });

                user.Addresses?.Add(new UserAddress
                {
                    City = createShipperDto.Address.City,
                    Street = createShipperDto.Address.Street,
                    Governorate = createShipperDto.Address.Governorate,
                    Details = createShipperDto.Address.Details,
                    GoogleMapAddressLink = createShipperDto.Address.GoogleMapAddressLink,
                    User = user,
                    UserID = user.Id
                });

                var CreateUserResult = await _userRepository.CreateUserAsync(user, createShipperDto.Password);

                if (!CreateUserResult.Success)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Fail(CreateUserResult.StatusCode, CreateUserResult.ErrorMessage);
                }

                var addShipperRoleResult = await _userRepository.AddRoleAsync(user, RolesEnum.Shipper);

                if (!addShipperRoleResult.Success)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Fail(addShipperRoleResult.StatusCode, addShipperRoleResult.ErrorMessage);
                }

                var shipper = new Shipper
                {
                    CompanyName = createShipperDto.CompanyName,
                    CompanyLink = createShipperDto.CompanyLink,
                    TypeOfProduction = createShipperDto.TypeOfProduction,
                    ShipperId = user.Id,
                };

                _context.Shippers.Add(shipper!);
                var saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
                }

                var sendEmailConfirmationLink = await _userRepository.
                    SendEmailConfirmationLinkAsync(
                        new RequestEmailConfirmationDto { ConfirmEmailUrl = createShipperDto.ConfirmEmailUrl, Email = createShipperDto.Email }
                    );

                if (!sendEmailConfirmationLink.Success)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Fail(sendEmailConfirmationLink.StatusCode, sendEmailConfirmationLink.ErrorMessage);
                }

                await transaction.CommitAsync();

                return OperationResult.Ok();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e.Message.ToString());
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
