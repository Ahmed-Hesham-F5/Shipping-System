using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using ShippingSystem.Results;
using System.Data;
using System.Transactions;

namespace ShippingSystem.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public ShipperRepository(AppDbContext context,IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<ValueOperationResult<AuthResponse>> AddShipperAsync(ShipperRegisterDto ShipperRegisterDto)
        {
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
                var CreateUserResult= await _userRepository.CreateUserAsync(user, ShipperRegisterDto.Password);
                if (!CreateUserResult.Success)
                    return ValueOperationResult<AuthResponse>.Fail(CreateUserResult.ErrorMessage);

                var addShipperRoleResult = await _userRepository.AddRoleAsync(user, RolesEnum.Shipper);
                if (!addShipperRoleResult.Success)
                    return ValueOperationResult<AuthResponse>.Fail(addShipperRoleResult.ErrorMessage);
             
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
                    return ValueOperationResult<AuthResponse>.Fail("Bad request");
           
                await transaction.CommitAsync();
                return ValueOperationResult<AuthResponse>.Ok(await _userRepository.GetUserTokensAsync(user));
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e.Message.ToString());
                return ValueOperationResult<AuthResponse>.Fail("Bad request");
            }
        }
    }
}
