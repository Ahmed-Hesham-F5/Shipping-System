using ShippingSystem.DTOs.ShipmentDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipmentRepository
    {
        Task<OperationResult> AddShipment(string userId, CreateShipmentDto shipmentDTO);
        Task<ValueOperationResult<PaginatedShipmentsDto>> GetAllShipments(string userId,
            ShipmentFiltrationParams filtrationParams, int pageNumber = 1, int pageSize = 9);
        Task<ValueOperationResult<ShipmentDetailsDto?>> GetShipmentById(string userId, int shipmentId);
        Task<ValueOperationResult<List<ToPickupShipmentListDto>>> GetShipmentsToPickup(string userId);
        Task<ValueOperationResult<List<ToReturnShipmentListDto>>> GetShipmentsToReturn(string userId);
        Task<ValueOperationResult<List<ToCancelShipmentListDto>>> GetShipmentsToCancel(string userId);
        Task<ValueOperationResult<ShipmentDetailsDto?>> UpdateShipment(string userId, int shipmentId, UpdateShipmentDto shipmentDTO);
        Task<OperationResult> DeleteShipment(string userId, int shipmentId);
        Task<OperationResult> UpdateShipmentStatus(string userId, int shipmentId, ShipmentStatusEnum newStatus, string? notes = null);
        Task<ValueOperationResult<ShipmentStatusStatisticsDto>> GetShipmentStatusStatistics(string userId);
        List<string> ShipmentStatus();
    }
}
