using ShippingSystem.DTOs;
using ShippingSystem.Enums;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<OperationResult> AddShipment(string userId, CreateShipmentDto shipmentDTO);
        Task<ValueOperationResult<List<ShipmentListDto>>> GetAllShipments(string userId);
        Task<ValueOperationResult<ShipmentDetailsDto?>> GetShipmentById(string userId, int id);
        Task<ValueOperationResult<List<ToPickupShipmentListDto>>> GetShipmentsToPickup(string userId);
        Task<ValueOperationResult<List<ToReturnShipmentListDto>>> GetShipmentsToReturn(string userId);
        Task<ValueOperationResult<ShipmentDetailsDto?>> UpdateShipment(string userId, int shipmentId, UpdateShipmentDto shipmentDTO);
        Task<OperationResult> DeleteShipment(string userId, int id);
        Task<OperationResult> UpdateShipmentStatus(string userId, int shipmentId, ShipmentStatusEnum newStatus, string? notes = null);
        Task<OperationResult> CreatePickupRequest(string userId, CreatePickupRequestDto pickupRequestDto);
        Task<OperationResult> CreateReturnRequest(string userId, CreateReturnRequestDto returnRequestDto);
        Task<ValueOperationResult<List<RequestListDto>>> GetAllRequests(string userId);
        Task<ValueOperationResult<ShipmentStatusStatisticsDto>> GetShipmentStatusStatistics(string userId);
    }
}
