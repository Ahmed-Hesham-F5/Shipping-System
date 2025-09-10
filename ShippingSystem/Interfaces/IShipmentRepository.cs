using ShippingSystem.DTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<OperationResult> AddShipment(string userId, AddShipmentDto shipmentDTO);
        Task<ValueOperationResult<List<GetShipmentsDTO>>> GetAllShipments(string userId);
        Task<ValueOperationResult<GetShipmentDetailsDTO?>> GetShipmentById(string userId, int id);
        Task<OperationResult> UpdateShipment(string userId, int id, AddShipmentDto shipmentDTO);
        Task<OperationResult> DeleteShipment(string userId, int id);
    }
}
