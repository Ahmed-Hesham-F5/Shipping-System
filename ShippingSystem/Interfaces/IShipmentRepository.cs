using ShippingSystem.DTO;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<bool> AddShipment(string userId, ShipmentDto shipmentDto);
        Task<List<GetShipmentsDto>> GetAllShipments(string userId);
        Task<GetShipmentDetailsDto?> GetShipmentById(string userId, int id);
        Task<bool> UpdateShipment(string userId, int id, ShipmentDto shipmentDto);
        Task<bool> DeleteShipment(string userId, int id);
    }
}
