using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShipmentRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<OperationResult> AddShipment(string userId, ShipmentDto shipmentDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = new Shipment
            {
                ShipperId = userId,
                ReceiverName = shipmentDto.ReceiverName,
                ReceiverPhone = shipmentDto.ReceiverPhone,
                ReceiverEmail = shipmentDto.ReceiverEmail,
                ReceiverAddress = new ReceiverAddress
                {
                    Street = shipmentDto.Street,
                    City = shipmentDto.City,
                    Country = shipmentDto.Country,
                    Details = shipmentDto.AddressDetails
                },
                ShipmentDescription = shipmentDto.ShipmentDescription,
                ShipmentWeight = shipmentDto.ShipmentWeight,
                ShipmentLength = shipmentDto.ShipmentLength,
                ShipmentWidth = shipmentDto.ShipmentWidth,
                ShipmentHeight = shipmentDto.ShipmentHeight,
                Quantity = shipmentDto.Quantity,
                ShipmentNotes = shipmentDto.ShipmentNotes,
                CashOnDeliveryEnabled = shipmentDto.CashOnDeliveryEnabled,
                OpenPackageOnDeliveryEnabled = shipmentDto.OpenPackageOnDeliveryEnabled,
                ExpressDeliveryEnabled = shipmentDto.ExpressDeliveryEnabled
            };

            _context.Shipments.Add(shipment);

            var addShipmentResult = await _context.SaveChangesAsync() > 0;

            if (!addShipmentResult)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            var shipmentStatus = ShipmentStatus.Create(shipment.Id,
                ShipmentStatusEnum.Pending.ToString(), "Shipment created");

            _context.ShipmentStatuses.Add(shipmentStatus);

            var addShipmentStatusResult = await _context.SaveChangesAsync() > 0;

            if (!addShipmentStatusResult)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<GetShipmentsDto>>> GetAllShipments(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<GetShipmentsDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipmentsList = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(shipment => new GetShipmentsDto
                {
                    Id = shipment.Id,
                    ReceiverName = shipment.ReceiverName,
                    ReceiverPhone = shipment.ReceiverPhone,
                    ReceiverEmail = shipment.ReceiverEmail,
                    ReceiverAddress = new ReceiverAddressDto
                    {
                        Street = shipment.ReceiverAddress.Street,
                        City = shipment.ReceiverAddress.City,
                        Country = shipment.ReceiverAddress.Country,
                        Details = shipment.ReceiverAddress.Details
                    },
                    ShipmentDescription = shipment.ShipmentDescription,
                    ShipmentWeight = shipment.ShipmentWeight,
                    ShipmentLength = shipment.ShipmentLength,
                    ShipmentWidth = shipment.ShipmentWidth,
                    ShipmentHeight = shipment.ShipmentHeight,
                    ShipmentVolume = shipment.ShipmentLength * shipment.ShipmentWidth * shipment.ShipmentHeight,
                    Quantity = shipment.Quantity,
                    ShipmentNotes = shipment.ShipmentNotes,
                    CashOnDeliveryEnabled = shipment.CashOnDeliveryEnabled,
                    OpenPackageOnDeliveryEnabled = shipment.OpenPackageOnDeliveryEnabled,
                    ExpressDeliveryEnabled = shipment.ExpressDeliveryEnabled,
                    CreatedAt = shipment.CreatedAt,
                    UpdatedAt = shipment.UpdatedAt,
                    ShipmentTrackingNumber = shipment.ShipmentTrackingNumber,
                    ShipmentStatuses = shipment.ShipmentStatuses
                        .Select(ss => new ShipmentStatusDto
                        {
                            Id = ss.Id,
                            Status = ss.Status,
                            Timestamp = ss.Timestamp,
                            Notes = ss.Notes
                        }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            ValueOperationResult<List<GetShipmentsDto>> shipments =
                ValueOperationResult<List<GetShipmentsDto>>.Ok(shipmentsList);

            return shipments;
        }

        public async Task<ValueOperationResult<GetShipmentDetailsDto?>> GetShipmentById(string userId, int id)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<GetShipmentDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var result = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (result == null)
                return ValueOperationResult<GetShipmentDetailsDto?>
                    .Fail(StatusCodes.Status403Forbidden, "Forbidden");

            var shipmentDetails = await _context.Shipments
            .Where(s => s.ShipperId == userId && s.Id == id)
            .Select(shipment => new GetShipmentDetailsDto
            {
                Id = shipment.Id,
                ReceiverName = shipment.ReceiverName,
                ReceiverPhone = shipment.ReceiverPhone,
                ReceiverAdditionalPhone = shipment.ReceiverAdditionalPhone,
                ReceiverEmail = shipment.ReceiverEmail,
                ReceiverAddress = new ReceiverAddressDto
                {
                    Street = shipment.ReceiverAddress.Street,
                    City = shipment.ReceiverAddress.City,
                    Country = shipment.ReceiverAddress.Country,
                    Details = shipment.ReceiverAddress.Details
                },
                ShipmentDescription = shipment.ShipmentDescription,
                ShipmentWeight = shipment.ShipmentWeight,
                ShipmentLength = shipment.ShipmentLength,
                ShipmentWidth = shipment.ShipmentWidth,
                ShipmentHeight = shipment.ShipmentHeight,
                ShipmentVolume = shipment.ShipmentLength * shipment.ShipmentWidth * shipment.ShipmentHeight,
                Quantity = shipment.Quantity,
                ShipmentNotes = shipment.ShipmentNotes,
                CashOnDeliveryEnabled = shipment.CashOnDeliveryEnabled,
                OpenPackageOnDeliveryEnabled = shipment.OpenPackageOnDeliveryEnabled,
                ExpressDeliveryEnabled = shipment.ExpressDeliveryEnabled,
                CreatedAt = shipment.CreatedAt,
                UpdatedAt = shipment.UpdatedAt,
                ShipmentTrackingNumber = shipment.ShipmentTrackingNumber,
                ShipmentStatuses = shipment.ShipmentStatuses
                    .Select(ss => new ShipmentStatusDto
                    {
                        Id = ss.Id,
                        Status = ss.Status,
                        Timestamp = ss.Timestamp,
                        Notes = ss.Notes
                    }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return ValueOperationResult<GetShipmentDetailsDto?>.Ok(shipmentDetails);
        }

        public async Task<OperationResult> UpdateShipment(string userId, int id, ShipmentDto shipmentDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Forbidden");

            shipment.ReceiverName = shipmentDto.ReceiverName;
            shipment.ReceiverPhone = shipmentDto.ReceiverPhone;
            shipment.ReceiverEmail = shipmentDto.ReceiverEmail;
            shipment.ReceiverAddress.Street = shipmentDto.Street;
            shipment.ReceiverAddress.City = shipmentDto.City;
            shipment.ReceiverAddress.Country = shipmentDto.Country;
            shipment.ReceiverAddress.Details = shipmentDto.AddressDetails;
            shipment.ShipmentDescription = shipmentDto.ShipmentDescription;
            shipment.ShipmentWeight = shipmentDto.ShipmentWeight;
            shipment.ShipmentLength = shipmentDto.ShipmentLength;
            shipment.ShipmentWidth = shipmentDto.ShipmentWidth;
            shipment.ShipmentHeight = shipmentDto.ShipmentHeight;
            shipment.Quantity = shipmentDto.Quantity;
            shipment.ShipmentNotes = shipmentDto.ShipmentNotes;
            shipment.CashOnDeliveryEnabled = shipmentDto.CashOnDeliveryEnabled;
            shipment.OpenPackageOnDeliveryEnabled = shipmentDto.OpenPackageOnDeliveryEnabled;
            shipment.ExpressDeliveryEnabled = shipmentDto.ExpressDeliveryEnabled;

            _context.Shipments.Update(shipment);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteShipment(string userId, int id)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Forbidden");

            _context.Shipments.Remove(shipment);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }
    }
}
