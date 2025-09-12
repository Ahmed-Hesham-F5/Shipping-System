using ShippingSystem.DTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<OperationResult> AddShipment(string userId, ShipmentRequestDto shipmentDTO);
        Task<ValueOperationResult<List<ShipmentListDto>>> GetAllShipments(string userId);
        Task<ValueOperationResult<ShipmentDetailsDto?>> GetShipmentById(string userId, int id);
        Task<OperationResult> UpdateShipment(string userId, int id, ShipmentRequestDto shipmentDTO);
        Task<OperationResult> DeleteShipment(string userId, int id);
    }
}
