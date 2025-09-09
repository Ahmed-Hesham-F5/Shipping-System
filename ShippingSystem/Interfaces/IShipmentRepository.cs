using ShippingSystem.DTO;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<OperationResult> AddShipment(string userId, ShipmentDto shipmentDto);
        Task<ValueOperationResult<List<GetShipmentsDto>>> GetAllShipments(string userId);
        Task<ValueOperationResult<GetShipmentDetailsDto?>> GetShipmentById(string userId, int id);
        Task<OperationResult> UpdateShipment(string userId, int id, ShipmentDto shipmentDto);
        Task<OperationResult> DeleteShipment(string userId, int id);
    }
}
