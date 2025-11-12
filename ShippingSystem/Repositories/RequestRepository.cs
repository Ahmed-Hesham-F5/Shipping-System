using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.RequestDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using static ShippingSystem.Helpers.DateTimeExtensions;

namespace ShippingSystem.Repositories
{
    public class RequestRepository(AppDbContext context,
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        IShipmentRepository shipmentRepository,
        IMapper mapper) : IRequestRepository
    {

        private readonly AppDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IShipmentRepository _shipmentRepository = shipmentRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationResult> CreatePickupRequest(string userId, CreatePickupRequestDto pickupRequestDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var validPickupStatuses = new List<string>
            {
                ShipmentStatusEnum.Pending.ToString(),
                ShipmentStatusEnum.Returned.ToString(),
            };

            if (pickupRequestDto.ShipmentIds.Count == 0)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No shipments selected for pickup");

            var validShipmentIds = await _context.Shipments
                .Where(s => s.ShipperId == userId && pickupRequestDto.ShipmentIds.Contains(s.Id))
                .Where(s => validPickupStatuses.Contains(
                    s.ShipmentStatuses
                    .OrderByDescending(ss => ss.Timestamp)
                    .Select(ss => ss.Status)
                    .FirstOrDefault()!)
                )
                .Select(s => s.Id)
                .ToListAsync();

            if (validShipmentIds.Count != pickupRequestDto.ShipmentIds.Count)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Some shipments are invalid or not found");

            var pickupRequest = _mapper.Map<PickupRequest>(pickupRequestDto);

            pickupRequest.UserId = userId;
            pickupRequest.RequestType = RequestTypeEnum.PickupRequest;
            pickupRequest.RequestStatus = RequestStatusEnum.Pending;
            pickupRequest.ShipmentsCount = validShipmentIds.Count;
            pickupRequest.CreatedAt = pickupRequest.UpdatedAt = UtcNowTrimmedToSeconds();

            await _context.PickupRequests.AddAsync(pickupRequest);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            foreach (var shipmentId in validShipmentIds)
                await _shipmentRepository.UpdateShipmentStatus(userId, shipmentId, ShipmentStatusEnum.InReviewForPickup, "");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> CreateReturnRequest(string userId, CreateReturnRequestDto returnRequestDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            if (returnRequestDto.ShipmentIds.Count == 0)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No shipments selected for return");

            var validShipmentIds = await _context.Shipments
                    .Where(s => s.ShipperId == userId && returnRequestDto.ShipmentIds.Contains(s.Id))
                    .Select(s => s.Id)
                    .ToListAsync();

            if (validShipmentIds.Count != returnRequestDto.ShipmentIds.Count)
                return OperationResult.Fail(StatusCodes.Status404NotFound,
                    "Some shipments are invalid or not found");

            var returnRequest = _mapper.Map<ReturnRequest>(returnRequestDto);

            returnRequest.UserId = userId;
            returnRequest.RequestType = RequestTypeEnum.ReturnRequest;
            returnRequest.RequestStatus = RequestStatusEnum.Pending;
            returnRequest.ShipmentsCount = validShipmentIds.Count;
            returnRequest.CreatedAt = returnRequest.UpdatedAt = UtcNowTrimmedToSeconds();

            await _context.ReturnRequests.AddAsync(returnRequest);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            foreach (var shipmentId in validShipmentIds)
                await _shipmentRepository.UpdateShipmentStatus(userId, shipmentId, ShipmentStatusEnum.InReviewForReturn, "");

            return OperationResult.Ok();
        }

        public async Task<OperationResult> CreateCancellationRequest(string userId, int requestId, CreateCancellationRequestDto cancellationRequestDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            if (cancellationRequestDto.ShipmentIds == null || cancellationRequestDto.ShipmentIds.Count == 0)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No shipments selected for cancellation");

            var request = await GetRequestInfo(userId, requestId);

            if (request == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Request not found");

            var requestType = request.RequestType!.Value;
            var requestStatus = request.RequestStatus!.Value;

            var validRequestStatuses = new List<RequestStatusEnum>
            {
                RequestStatusEnum.InReview,
                RequestStatusEnum.Approved,
                RequestStatusEnum.InProgress
            };

            if (!validRequestStatuses.Contains(requestStatus))
                return OperationResult.Fail(StatusCodes.Status409Conflict,
                    "Shipment can only be requested to be canceled when the request status is InReview, Approved, or InProgress.");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            List<int> validShipmentIds = new List<int>();

            try
            {
                if (requestType == RequestTypeEnum.PickupRequest)
                {
                    validShipmentIds = await GetValidPickupShipmentIds(requestId, cancellationRequestDto.ShipmentIds);

                    if (validShipmentIds.Count != cancellationRequestDto.ShipmentIds.Count)
                        return OperationResult.Fail(StatusCodes.Status404NotFound,
                            "Some shipments are invalid or not found in the specified pickup request");
                }
                else if (requestType == RequestTypeEnum.ReturnRequest)
                {
                    validShipmentIds = await GetValidReturnShipmentIds(requestId, cancellationRequestDto.ShipmentIds);

                    if (validShipmentIds.Count != cancellationRequestDto.ShipmentIds.Count)
                        return OperationResult.Fail(StatusCodes.Status404NotFound,
                            "Some shipments are invalid or not found in the specified return request");
                }

                var cancellationRequest = _mapper.Map<CancellationRequest>(cancellationRequestDto);
                cancellationRequest.UserId = userId;
                cancellationRequest.RequestType = RequestTypeEnum.CancellationRequest;
                cancellationRequest.RequestStatus = RequestStatusEnum.Pending;
                cancellationRequest.ShipmentsCount = validShipmentIds.Count;
                cancellationRequest.CreatedAt = cancellationRequest.UpdatedAt = UtcNowTrimmedToSeconds();

                await _context.CancellationRequests.AddAsync(cancellationRequest);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred. Please try again later.");

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }

            foreach (var shipmentId in validShipmentIds)
                await _shipmentRepository.UpdateShipmentStatus(userId, shipmentId, ShipmentStatusEnum.InReviewForCancellation, "");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<RequestListDto>>> GetAllRequests(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<RequestListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var allRequests = await _context.Requests
                .Where(r => r.UserId == userId)
                .ProjectTo<RequestListDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<RequestListDto>>.Ok(allRequests);
        }

        private async Task<RequestInfoDto?> GetRequestInfo(string userId, int requestId)
        {
            var request = await _context.Requests
                .Where(r => r.Id == requestId && r.UserId == userId)
                .Select(r => new RequestInfoDto
                {
                    RequestType = r.RequestType,
                    RequestStatus = r.RequestStatus
                })
                .FirstOrDefaultAsync();

            return request;
        }

        private async Task<List<int>> GetValidPickupShipmentIds(int pickupRequestId, List<int> shipmentIds)
        {
            var validShipmentStatuses = new List<string>
            {
                ShipmentStatusEnum.InReviewForPickup.ToString(),
                ShipmentStatusEnum.WaitingForPickup.ToString()
            };

            return await _context.PickupRequestShipments
                            .Where(prs => prs.PickupRequestId == pickupRequestId)
                            .Where(prs => shipmentIds.Contains(prs.ShipmentId))
                            .Where(prs => validShipmentStatuses.Contains(
                                prs.Shipment!
                                .ShipmentStatuses
                                .OrderByDescending(ss => ss.Timestamp)
                                .Select(ss => ss.Status)
                                .FirstOrDefault()!)
                            )
                            .Select(prs => prs.ShipmentId)
                            .ToListAsync();
        }

        private async Task<List<int>> GetValidReturnShipmentIds(int returnRequestId, List<int> shipmentIds)
        {
            var validShipmentStatuses = new List<string>
            {
                ShipmentStatusEnum.InReviewForReturn.ToString(),
                ShipmentStatusEnum.WaitingForReturn.ToString()
            };

            return await _context.ReturnRequestShipments
                            .Where(r => r.ReturnRequestId == returnRequestId)
                            .Where(r => shipmentIds.Contains(r.ShipmentId))
                            .Where(r => validShipmentStatuses.Contains(
                                r.Shipment!
                                .ShipmentStatuses
                                .OrderByDescending(ss => ss.Timestamp)
                                .Select(ss => ss.Status)
                                .FirstOrDefault()!)
                            )
                            .Select(r => r.ShipmentId)
                            .ToListAsync();
        }

        public async Task<ValueOperationResult<PickupRequestDetailsDto?>> GetPickupRequestById(string userId, int pickupRequestId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<PickupRequestDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var pickupRequest = await _context.PickupRequests
                .Where(r => r.Id == pickupRequestId && r.UserId == userId)
                .ProjectTo<PickupRequestDetailsDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (pickupRequest == null)
                return ValueOperationResult<PickupRequestDetailsDto?>
                    .Fail(StatusCodes.Status404NotFound, "Pickup request not found");

            pickupRequest.Role = (await _userRepository.GetUserRoleAsync(userId)).Value!;

            return ValueOperationResult<PickupRequestDetailsDto?>.Ok(pickupRequest);
        }

        public async Task<ValueOperationResult<ReturnRequestDetailsDto?>> GetReturnRequestById(string userId, int returnRequestId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ReturnRequestDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var returnRequest = await _context.ReturnRequests
                .Where(r => r.Id == returnRequestId && r.UserId == userId)
                .ProjectTo<ReturnRequestDetailsDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (returnRequest == null)
                return ValueOperationResult<ReturnRequestDetailsDto?>
                    .Fail(StatusCodes.Status404NotFound, "Return request not found");

            returnRequest.Role = (await _userRepository.GetUserRoleAsync(userId)).Value!;

            return ValueOperationResult<ReturnRequestDetailsDto?>.Ok(returnRequest);
        }

        public async Task<ValueOperationResult<CancellationRequestDetailsDto?>> GetCancellationRequestById(string userId, int cancellationRequestId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<CancellationRequestDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var cancellationRequest = await _context.CancellationRequests
                .Where(r => r.Id == cancellationRequestId && r.UserId == userId)
                .ProjectTo<CancellationRequestDetailsDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (cancellationRequest == null)
                return ValueOperationResult<CancellationRequestDetailsDto?>
                    .Fail(StatusCodes.Status404NotFound, "Cancellation request not found");

            cancellationRequest.Role = (await _userRepository.GetUserRoleAsync(userId)).Value!;

            return ValueOperationResult<CancellationRequestDetailsDto?>.Ok(cancellationRequest);
        }

        public async Task<OperationResult> UpdateRequestStatus(string userId, int requestId, RequestStatusEnum newStatus, string? notes = null)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId && r.UserId == userId);

            if (request == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Request not found");

            var getUserRoleResult = await _userRepository.GetUserRoleAsync(userId);

            if (!getUserRoleResult.Success)
                return OperationResult.Fail(getUserRoleResult.StatusCode, getUserRoleResult.ErrorMessage);

            if (getUserRoleResult.Value == RolesEnum.Shipper.ToString() && request.UserId != userId)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Request not found");

            if (request.RequestStatus.ToString() == newStatus.ToString())
                return OperationResult.Fail(StatusCodes.Status400BadRequest,
                    "The request already has the specified status.");

            request.RequestStatus = newStatus;
            request.UpdatedAt = UtcNowTrimmedToSeconds();
            _context.Requests.Update(request);
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }
    }
}
