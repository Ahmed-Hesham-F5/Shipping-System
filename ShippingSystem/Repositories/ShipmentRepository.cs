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

        public async Task<OperationResult> AddShipment(string userId, AddShipmentDto shipmentDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = new Shipment
            {
                ShipperId = userId,
                ReceiverName = shipmentDTO.ReceiverName,
                ReceiverPhone = shipmentDTO.ReceiverPhone,
                ReceiverEmail = shipmentDTO.ReceiverEmail,
                ReceiverAddress = new ReceiverAddress
                {
                    Street = shipmentDTO.Street,
                    City = shipmentDTO.City,
                    Country = shipmentDTO.Country,
                    Details = shipmentDTO.AddressDetails
                },
                ShipmentDescription = shipmentDTO.ShipmentDescription,
                ShipmentWeight = shipmentDTO.ShipmentWeight,
                ShipmentLength = shipmentDTO.ShipmentLength,
                ShipmentWidth = shipmentDTO.ShipmentWidth,
                ShipmentHeight = shipmentDTO.ShipmentHeight,
                Quantity = shipmentDTO.Quantity,
                ShipmentNotes = shipmentDTO.ShipmentNotes,
                CashOnDeliveryEnabled = shipmentDTO.CashOnDeliveryEnabled,
                OpenPackageOnDeliveryEnabled = shipmentDTO.OpenPackageOnDeliveryEnabled,
                ExpressDeliveryEnabled = shipmentDTO.ExpressDeliveryEnabled
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

        public async Task<ValueOperationResult<List<GetShipmentsDTO>>> GetAllShipments(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<GetShipmentsDTO>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipmentsList = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(shipment => new GetShipmentsDTO
                {
                    Id = shipment.Id,
                    ReceiverName = shipment.ReceiverName,
                    ReceiverPhone = shipment.ReceiverPhone,
                    ReceiverEmail = shipment.ReceiverEmail,
                    ReceiverAddress = new AddressDTO
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
                        .Select(ss => new ShipmentStatusDTO
                        {
                            Id = ss.Id,
                            Status = ss.Status,
                            Timestamp = ss.Timestamp,
                            Notes = ss.Notes
                        }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            ValueOperationResult<List<GetShipmentsDTO>> shipments =
                ValueOperationResult<List<GetShipmentsDTO>>.Ok(shipmentsList);

            return shipments;
        }

        public async Task<ValueOperationResult<GetShipmentDetailsDTO?>> GetShipmentById(string userId, int id)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<GetShipmentDetailsDTO?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var result = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (result == null)
                return ValueOperationResult<GetShipmentDetailsDTO?>
                    .Fail(StatusCodes.Status403Forbidden, "Forbidden");

            var shipmentDetails = await _context.Shipments
            .Where(s => s.ShipperId == userId && s.Id == id)
            .Select(shipment => new GetShipmentDetailsDTO
            {
                Id = shipment.Id,
                ReceiverName = shipment.ReceiverName,
                ReceiverPhone = shipment.ReceiverPhone,
                ReceiverAdditionalPhone = shipment.ReceiverAdditionalPhone,
                ReceiverEmail = shipment.ReceiverEmail,
                ReceiverAddress = new AddressDTO
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
                    .Select(ss => new ShipmentStatusDTO
                    {
                        Id = ss.Id,
                        Status = ss.Status,
                        Timestamp = ss.Timestamp,
                        Notes = ss.Notes
                    }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return ValueOperationResult<GetShipmentDetailsDTO?>.Ok(shipmentDetails);
        }

        public async Task<OperationResult> UpdateShipment(string userId, int id, AddShipmentDto shipmentDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Forbidden");

            shipment.ReceiverName = shipmentDTO.ReceiverName;
            shipment.ReceiverPhone = shipmentDTO.ReceiverPhone;
            shipment.ReceiverEmail = shipmentDTO.ReceiverEmail;
            shipment.ReceiverAddress.Street = shipmentDTO.Street;
            shipment.ReceiverAddress.City = shipmentDTO.City;
            shipment.ReceiverAddress.Country = shipmentDTO.Country;
            shipment.ReceiverAddress.Details = shipmentDTO.AddressDetails;
            shipment.ShipmentDescription = shipmentDTO.ShipmentDescription;
            shipment.ShipmentWeight = shipmentDTO.ShipmentWeight;
            shipment.ShipmentLength = shipmentDTO.ShipmentLength;
            shipment.ShipmentWidth = shipmentDTO.ShipmentWidth;
            shipment.ShipmentHeight = shipmentDTO.ShipmentHeight;
            shipment.Quantity = shipmentDTO.Quantity;
            shipment.ShipmentNotes = shipmentDTO.ShipmentNotes;
            shipment.CashOnDeliveryEnabled = shipmentDTO.CashOnDeliveryEnabled;
            shipment.OpenPackageOnDeliveryEnabled = shipmentDTO.OpenPackageOnDeliveryEnabled;
            shipment.ExpressDeliveryEnabled = shipmentDTO.ExpressDeliveryEnabled;

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
