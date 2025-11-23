using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.PhoneNumberDTOs;
using ShippingSystem.DTOs.ShipperDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository(AppDbContext context,
        IUserRepository userRepository,
        IEmailService emailService,
        UserManager<ApplicationUser> userManager
        ) : IShipperRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

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

        public async Task<ValueOperationResult<ShipperProfileDto>> GetShipperProfileAsync(string shipperId)
        {
            var shipper = await _context.Shippers
               .Include(s => s.User.Phones)
               .Include(s => s.User.Addresses)
               .AsSplitQuery()
               .FirstOrDefaultAsync(s => s.ShipperId == shipperId);

            if (shipper == null)
                return ValueOperationResult<ShipperProfileDto>.Fail(StatusCodes.Status404NotFound, "Shipper not found.");

            var shipperProfile = new ShipperProfileDto
            {
                ShipperId = shipper.ShipperId,
                FirstName = shipper.User.FirstName,
                LastName = shipper.User.LastName,
                Email = shipper.User.Email!,
                Phones = [.. shipper.User.Phones!.Select(p => p.PhoneNumber)],
                Addresses = [.. shipper.User.Addresses!.Select(a => new ShipperAddressListDto
                {
                    Id = a.Id,
                    City = a.City,
                    Street = a.Street,
                    Governorate = a.Governorate,
                    Details = a.Details,
                    GoogleMapAddressLink = a.GoogleMapAddressLink
                })],
                CompanyName = shipper.CompanyName,
                CompanyLink = shipper.CompanyLink,
                TypeOfProduction = shipper.TypeOfProduction
            };

            return ValueOperationResult<ShipperProfileDto>.Ok(shipperProfile);
        }

        public async Task<OperationResult> AddShipperAddressAsync(string shipperId, AddressDto addressDto)
        {
            var shipper = await _context.Shippers
                .Include(s => s.User.Addresses)
                .FirstOrDefaultAsync(s => s.ShipperId == shipperId);

            if (shipper == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipper not found.");

            var address = new UserAddress
            {
                City = addressDto.City,
                Street = addressDto.Street,
                Governorate = addressDto.Governorate,
                Details = addressDto.Details,
                GoogleMapAddressLink = addressDto.GoogleMapAddressLink
            };

            shipper.User.Addresses!.Add(address);
            var saveResult = await _context.SaveChangesAsync();

            if (saveResult <= 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> UpdateShipperAddressAsync(string shipperId, int addressId, AddressDto addressDto)
        {
            var shipper = await _context.Shippers
                .Include(s => s.User.Addresses)
                .FirstOrDefaultAsync(s => s.ShipperId == shipperId);

            if (shipper == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipper not found.");

            var address = shipper.User.Addresses!.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Address not found.");

            bool noChanges = true;

            foreach (var prop in typeof(AddressDto).GetProperties())
            {
                var dtoValue = prop.GetValue(addressDto);
                var entityValue = typeof(UserAddress)
                                    .GetProperty(prop.Name)?
                                    .GetValue(address);

                if (!Equals(dtoValue, entityValue))
                {
                    noChanges = false;
                    break;
                }
            }

            if (noChanges)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No changes detected.");

            address.City = addressDto.City;
            address.Street = addressDto.Street;
            address.Governorate = addressDto.Governorate;
            address.Details = addressDto.Details;
            address.GoogleMapAddressLink = addressDto.GoogleMapAddressLink;

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult <= 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteShipperAddressAsync(string shipperId, int addressId)
        {
            var shipper = await _context.Shippers
                .Include(s => s.User.Addresses)
                .FirstOrDefaultAsync(s => s.ShipperId == shipperId);

            if (shipper == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipper not found.");

            var address = shipper.User.Addresses!.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Address not found.");

            if (shipper.User.Addresses!.Count <= 1)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "At least one address must be maintained.");

            shipper.User.Addresses!.Remove(address);
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult <= 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> AddPhoneNumberAsync(string shipperId, PhoneNumberDto phoneNumberDto)
        {
            if (await _userManager.FindByIdAsync(shipperId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var phoneNumber = new UserPhone
            {
                PhoneNumber = phoneNumberDto.PhoneNumber,
                UserId = shipperId
            };

            _context.UserPhones.Add(phoneNumber);

            var saveResult = await _context.SaveChangesAsync();
            
            if (saveResult <= 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeletePhoneNumberAsync(string shipperId, PhoneNumberDto phoneNumberDto)
        {
            if (await _userManager.FindByIdAsync(shipperId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var phoneNumber = await _context.UserPhones
                .FirstOrDefaultAsync(p => p.UserId == shipperId && p.PhoneNumber == phoneNumberDto.PhoneNumber);

            if (phoneNumber == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Phone number not found.");

            if ((await _context.UserPhones.CountAsync(p => p.UserId == shipperId)) <= 1)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "At least one phone number must be maintained.");

            _context.UserPhones.Remove(phoneNumber);

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult <= 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }
    }
}
