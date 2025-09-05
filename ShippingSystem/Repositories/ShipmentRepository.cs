using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;

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

        public async Task<bool> AddShipment(string userId, ShipmentDto shipmentDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return false;

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
                return false;


            var shipmentStatus = ShipmentStatus.Create(shipment.Id,
                ShipmentStatusEnum.Pending.ToString(), "تم إنشاء الشحنة");

            _context.ShipmentStatuses.Add(shipmentStatus);

            var addShipmentStatusResult = await _context.SaveChangesAsync() > 0;

            if (!addShipmentStatusResult)
                return false;

            return true;
        }

        public async Task<List<GetShipmentsDto>> GetAllShipments(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return new List<GetShipmentsDto>();

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

            return shipmentsList;
        }

        public async Task<GetShipmentDetailsDto?> GetShipmentById(string userId, int id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var shipment = await _context.Shipments
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

            return shipment;
        }

        public async Task<bool> UpdateShipment(string userId, int id, ShipmentDto shipmentDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
                return false;

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);
           
            if (shipment == null)
                return false;

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
           
            return result;
        }

        public async Task<bool> DeleteShipment(string userId, int id)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return false;

            _context.Shipments.Remove(shipment);

            var result = await _context.SaveChangesAsync() > 0;

            return result;
        }
    }
}
