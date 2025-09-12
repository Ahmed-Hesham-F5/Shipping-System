using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs;
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

        public async Task<OperationResult> AddShipment(string userId, ShipmentRequestDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = new Shipment
            {
                ShipperId = userId,
                ReceiverName = shipmentRequestDTO.ReceiverName,
                ReceiverPhone = shipmentRequestDTO.ReceiverPhone,
                ReceiverEmail = shipmentRequestDTO.ReceiverEmail,
                ReceiverAddress = new ReceiverAddress
                {
                    Street = shipmentRequestDTO.Street,
                    City = shipmentRequestDTO.City,
                    Country = shipmentRequestDTO.Country,
                    Details = shipmentRequestDTO.AddressDetails
                },
                ShipmentDescription = shipmentRequestDTO.ShipmentDescription,
                ShipmentWeight = shipmentRequestDTO.ShipmentWeight,
                ShipmentLength = shipmentRequestDTO.ShipmentLength,
                ShipmentWidth = shipmentRequestDTO.ShipmentWidth,
                ShipmentHeight = shipmentRequestDTO.ShipmentHeight,
                Quantity = shipmentRequestDTO.Quantity,
                ShipmentNotes = shipmentRequestDTO.ShipmentNotes,
                CashOnDeliveryEnabled = shipmentRequestDTO.CashOnDeliveryEnabled,
                OpenPackageOnDeliveryEnabled = shipmentRequestDTO.OpenPackageOnDeliveryEnabled,
                ExpressDeliveryEnabled = shipmentRequestDTO.ExpressDeliveryEnabled,
                CollectionAmount = shipmentRequestDTO.CollectionAmount
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

        public async Task<ValueOperationResult<List<ShipmentListDto>>> GetAllShipments(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<ShipmentListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var AllShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(shipment => new ShipmentListDto
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
                    CollectionAmount = shipment.CollectionAmount,
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

            return ValueOperationResult<List<ShipmentListDto>>.Ok(AllShipments);
        }

        public async Task<ValueOperationResult<ShipmentDetailsDto?>> GetShipmentById(string userId, int id)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var result = await _context.Shipments.Include(s => s.ShipmentStatuses)
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (result == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status403Forbidden, "Forbidden");

            ShipmentDetailsDto ShipmentDetails = new ShipmentDetailsDto
            {
                Id = result.Id,
                ReceiverName = result.ReceiverName,
                ReceiverPhone = result.ReceiverPhone,
                ReceiverAdditionalPhone = result.ReceiverAdditionalPhone,
                ReceiverEmail = result.ReceiverEmail,
                ReceiverAddress = new ReceiverAddressDto
                {
                    Street = result.ReceiverAddress.Street,
                    City = result.ReceiverAddress.City,
                    Country = result.ReceiverAddress.Country,
                    Details = result.ReceiverAddress.Details
                },
                ShipmentDescription = result.ShipmentDescription,
                ShipmentWeight = result.ShipmentWeight,
                ShipmentLength = result.ShipmentLength,
                ShipmentWidth = result.ShipmentWidth,
                ShipmentHeight = result.ShipmentHeight,
                ShipmentVolume = result.ShipmentLength * result.ShipmentWidth * result.ShipmentHeight,
                Quantity = result.Quantity,
                ShipmentNotes = result.ShipmentNotes,
                CashOnDeliveryEnabled = result.CashOnDeliveryEnabled,
                OpenPackageOnDeliveryEnabled = result.OpenPackageOnDeliveryEnabled,
                ExpressDeliveryEnabled = result.ExpressDeliveryEnabled,
                CollectionAmount = result.CollectionAmount,
                ShippingCost = result.ShippingCost,
                AdditionalWeight = result.AdditionalWeight,
                AdditionalWeightCost = result.AdditionalWeightCost,
                CollectionFee = result.CollectionFee,
                AdditionalCost = result.AdditionalCost,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                ShipmentTrackingNumber = result.ShipmentTrackingNumber,
                ShipmentStatuses = result.ShipmentStatuses
                    .Select(ss => new ShipmentStatusDto
                    {
                        Id = ss.Id,
                        Status = ss.Status,
                        Timestamp = ss.Timestamp,
                        Notes = ss.Notes
                    }).ToList()
            };

            return ValueOperationResult<ShipmentDetailsDto?>.Ok(ShipmentDetails);
        }

        public async Task<OperationResult> UpdateShipment(string userId, int id, ShipmentRequestDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Forbidden");

            shipment.ReceiverName = shipmentRequestDTO.ReceiverName;
            shipment.ReceiverPhone = shipmentRequestDTO.ReceiverPhone;
            shipment.ReceiverEmail = shipmentRequestDTO.ReceiverEmail;
            shipment.ReceiverAddress.Street = shipmentRequestDTO.Street;
            shipment.ReceiverAddress.City = shipmentRequestDTO.City;
            shipment.ReceiverAddress.Country = shipmentRequestDTO.Country;
            shipment.ReceiverAddress.Details = shipmentRequestDTO.AddressDetails;
            shipment.ShipmentDescription = shipmentRequestDTO.ShipmentDescription;
            shipment.ShipmentWeight = shipmentRequestDTO.ShipmentWeight;
            shipment.ShipmentLength = shipmentRequestDTO.ShipmentLength;
            shipment.ShipmentWidth = shipmentRequestDTO.ShipmentWidth;
            shipment.ShipmentHeight = shipmentRequestDTO.ShipmentHeight;
            shipment.Quantity = shipmentRequestDTO.Quantity;
            shipment.ShipmentNotes = shipmentRequestDTO.ShipmentNotes;
            shipment.CashOnDeliveryEnabled = shipmentRequestDTO.CashOnDeliveryEnabled;
            shipment.OpenPackageOnDeliveryEnabled = shipmentRequestDTO.OpenPackageOnDeliveryEnabled;
            shipment.ExpressDeliveryEnabled = shipmentRequestDTO.ExpressDeliveryEnabled;
            shipment.CollectionAmount = shipmentRequestDTO.CollectionAmount;

            var hasChanges = _context.Entry(shipment).State == EntityState.Modified
                     || _context.Entry(shipment.ReceiverAddress).State == EntityState.Modified;

            if (!hasChanges)
                return OperationResult.Fail(StatusCodes.Status304NotModified, "No changes detected.");

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
