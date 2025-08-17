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
            if (_userManager.FindByIdAsync(userId) == null)
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
                ShipmentNotes = shipmentDto.ShipmentNotes,
            };

            _context.Shipments.Add(shipment);

            var addShipmentResult = await _context.SaveChangesAsync() > 0;

            if (!addShipmentResult)
                return false;


            var shipmentStatus = ShipmentStatus.Create(shipment.Id,
                ShipmentStatusEnum.Pending.ToString(), null);

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
                    ShipperId = shipment.ShipperId,
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
    }
}
