using ShippingSystem.DTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<OperationResult> AddShipment(string userId, ShipmentFromRequestDto shipmentDTO);
        Task<ValueOperationResult<List<ShipmentListDto>>> GetAllShipments(string userId);
        Task<ValueOperationResult<ShipmentDetailsDto?>> GetShipmentById(string userId, int id);
        Task<ValueOperationResult<ShipmentDetailsDto?>> UpdateShipment(string userId, int id, ShipmentFromRequestDto shipmentDTO);
        Task<OperationResult> DeleteShipment(string userId, int id);
    }
}
