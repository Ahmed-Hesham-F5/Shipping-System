using ShippingSystem.DTO;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<bool> AddShipment(string userId, ShipmentDto shipmentDto);
        Task<List<GetShipmentsDto>> GetAllShipments(string userId);
    }
}
