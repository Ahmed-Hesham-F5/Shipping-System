using ShippingSystem.DTO;
using ShippingSystem.Models;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        public Task<bool> AddShipment(string userId, ShipmentDto shipmentDto);
        public Task<List<GetShipmentsDto>> GetAllShipments(string userId);
    }
}
