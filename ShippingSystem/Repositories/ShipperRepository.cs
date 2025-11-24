using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.EmailDTOs;
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
        UserManager<ApplicationUser> userManager,
        IMapper mapper) : IShipperRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

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
                    UserId = user.Id
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
                Addresses = [.. shipper.User.Addresses!.Select(a => _mapper.Map<ShipperAddressListDto>(a))],
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

            var address = _mapper.Map<UserAddress>(addressDto);

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

        public async Task<OperationResult> ChangeEmailRequestAsync(string shipperId, ChangeEmailRequestDto changeEmailRequestDto)
        {
            var user = await _userManager.FindByIdAsync(shipperId);
            if (user == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, changeEmailRequestDto.NewEmail);

            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            var sendChangeEmailLink = await _emailService.SendAsync(
                changeEmailRequestDto.NewEmail,
                "Change Email Confirmation",
                _emailService.ChangeEmailConfirmationBody(
                    changeEmailRequestDto.ConfirmNewEmailUrl,
                    changeEmailRequestDto.NewEmail,
                    user.Email!,
                    encodedToken
                )
               );

            if (!sendChangeEmailLink.Success)
                return OperationResult.Fail(sendChangeEmailLink.StatusCode, sendChangeEmailLink.ErrorMessage);

            return OperationResult.Ok();
        }

        public async Task<OperationResult> ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(changeEmailDto.OldEmail);
            if (user == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var decodedToken = System.Web.HttpUtility.UrlDecode(changeEmailDto.Token);

            var changeEmailResult = await _userManager.ChangeEmailAsync(user, changeEmailDto.NewEmail, decodedToken);

            if (!changeEmailResult.Succeeded)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");


            user.UserName = changeEmailDto.NewEmail;
            var updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> UpdateCompanyInformationAsync(string shipperId, UpdateCompanyInfoDto updateCompanyInfoDto)
        {
            var shipper = await _context.Shippers.FirstOrDefaultAsync(s => s.ShipperId == shipperId);
            if (shipper == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            bool noChanges = true;

            foreach (var prop in typeof(UpdateCompanyInfoDto).GetProperties())
            {
                var dtoValue = prop.GetValue(updateCompanyInfoDto);
                var entityValue = typeof(Shipper)
                                    .GetProperty(prop.Name)?
                                    .GetValue(shipper);

                if (!Equals(dtoValue, entityValue))
                {
                    noChanges = false;
                    break;
                }
            }

            if (noChanges)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No changes detected.");

            shipper.CompanyName = updateCompanyInfoDto.CompanyName;
            shipper.CompanyLink = updateCompanyInfoDto.CompanyLink;
            shipper.TypeOfProduction = updateCompanyInfoDto.TypeOfProduction;
            
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult <= 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            
            return OperationResult.Ok();
        }

        public async Task<OperationResult> UpdateShipperNameAsync(string shipperId, UpdateShipperNameDto updateShipperNameDto)
        {
            var shipper = await _userManager.FindByIdAsync(shipperId);
            if (shipper == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            bool noChanges = true;

            foreach (var prop in typeof(UpdateShipperNameDto).GetProperties())
            {
                var dtoValue = prop.GetValue(updateShipperNameDto);
                var entityValue = typeof(ApplicationUser)
                                    .GetProperty(prop.Name)?
                                    .GetValue(shipper);
                if (!Equals(dtoValue, entityValue))
                {
                    noChanges = false;
                    break;
                }
            }

            if (noChanges)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No changes detected.");

            shipper.FirstName = updateShipperNameDto.FirstName;
            shipper.LastName = updateShipperNameDto.LastName;

            var updateResult = await _userManager.UpdateAsync(shipper);
            if (!updateResult.Succeeded)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<AddressDto>>> GetShipperAddressesAsync(string shipperId)
        {
            if (await _userManager.FindByIdAsync(shipperId) == null)
                return ValueOperationResult<List<AddressDto>>.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var shipperAddresses = await _context.UserAddresses
                .Where(a => a.UserId == shipperId)
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return ValueOperationResult<List<AddressDto>>.Ok(shipperAddresses);
        }
    }
}
