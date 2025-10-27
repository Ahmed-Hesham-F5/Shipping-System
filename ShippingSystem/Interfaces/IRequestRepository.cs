using ShippingSystem.DTOs.RequestDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IRequestRepository
    {
        Task<OperationResult> CreatePickupRequest(string userId, CreatePickupRequestDto pickupRequestDto);
        Task<OperationResult> CreateReturnRequest(string userId, CreateReturnRequestDto returnRequestDto);
        Task<OperationResult> CreateCancellationRequest(string userId, int requestId, CreateCancellationRequestDto cancellationRequestDto);
        Task<OperationResult> CreateRescheduleRequest(string userId, CreateRescheduleRequestDto rescheduleRequestDto);
        Task<ValueOperationResult<List<RequestListDto>>> GetAllRequests(string userId);
        Task<ValueOperationResult<PickupRequestDetailsDto?>> GetPickupRequestById(string userId, int pickupRequestId);
        Task<ValueOperationResult<ReturnRequestDetailsDto?>> GetReturnRequestById(string userId, int returnRequestId);
        Task<ValueOperationResult<CancellationRequestDetailsDto?>> GetCancellationRequestById(string userId, int cancellationRequestId);
        Task<ValueOperationResult<RescheduleRequestDetailsDto?>> GetRescheduleRequestById(string userId, int rescheduleRequestId);
    }
}
