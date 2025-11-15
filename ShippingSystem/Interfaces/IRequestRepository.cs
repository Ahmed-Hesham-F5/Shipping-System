using ShippingSystem.DTOs.RequestDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IRequestRepository
    {
        Task<OperationResult> CreatePickupRequest(string userId, CreatePickupRequestDto pickupRequestDto);
        Task<OperationResult> CreateReturnRequest(string userId, CreateReturnRequestDto returnRequestDto);
        Task<OperationResult> CreateCancellationRequest(string userId, int requestId, CreateCancellationRequestDto cancellationRequestDto);
        Task<ValueOperationResult<List<RequestListDto>>> GetAllRequests(string userId);
        Task<ValueOperationResult<PickupRequestDetailsDto?>> GetPickupRequestById(string userId, int pickupRequestId);
        Task<ValueOperationResult<ReturnRequestDetailsDto?>> GetReturnRequestById(string userId, int returnRequestId);
        Task<ValueOperationResult<CancellationRequestDetailsDto?>> GetCancellationRequestById(string userId, int cancellationRequestId);
        Task<OperationResult> UpdateRequestStatus(string userId, int requestId, RequestStatusEnum newStatus, string? notes = null);
        Task<OperationResult> CreateExchangeRequest(string userId, CreateExchangeRequestDto exchangeRequestDto);
        Task<ValueOperationResult<ExchangeRequestDetailsDto?>> GetExchangeRequestById(string userId, int exchangeRequestId);
    }
}
