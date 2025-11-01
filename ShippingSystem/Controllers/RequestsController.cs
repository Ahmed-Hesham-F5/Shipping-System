using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.RequestDTOs;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;
using System.Security.Claims;

namespace ShippingSystem.Controllers
{
    [Authorize(Roles = "Shipper")]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        public RequestsController(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        [HttpPost("pickup-requests")]
        public async Task<IActionResult> PickupRequest([FromBody] CreatePickupRequestDto pickupRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.CreatePickupRequest(userId, pickupRequestDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<string>(true, "Pickup request created successfully."));
        }

        [HttpGet("getAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.GetAllRequests(userId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<RequestListDto>> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpPost("return-request")]
        public async Task<IActionResult> ReturnRequest([FromBody] CreateReturnRequestDto returnRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.CreateReturnRequest(userId, returnRequestDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<string>(true, "Return request created successfully."));
        }

        [HttpPost("cancellation-request/{requestId}")]
        public async Task<IActionResult> CancellationRequest(int requestId, [FromBody] CreateCancellationRequestDto cancellationRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.CreateCancellationRequest(userId, requestId, cancellationRequestDto);

            return StatusCode(result.StatusCode,
                new ApiResponse<string>(result.Success, result.Success
                    ? "Cancellation request created successfully."
                    : result.ErrorMessage));
        }

        [HttpGet("pickup-requests/{pickupRequestId}")]
        public async Task<IActionResult> GetPickupRequest(int pickupRequestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.GetPickupRequestById(userId, pickupRequestId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<PickupRequestDetailsDto>(true, null!, result.Value!));
        }

        [HttpGet("return-requests/{returnRequestId}")]
        public async Task<IActionResult> GetReturnRequest(int returnRequestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.GetReturnRequestById(userId, returnRequestId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<ReturnRequestDetailsDto>(true, null!, result.Value!));
        }

        [HttpGet("cancellation-requests/{cancellationRequestId}")]
        public async Task<IActionResult> GetCancellationRequest(int cancellationRequestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.GetCancellationRequestById(userId, cancellationRequestId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<CancellationRequestDetailsDto>(true, null!, result.Value!));
        }

        [HttpPost("reschedule-requests")]
        public async Task<IActionResult> RescheduleRequest([FromBody] CreateRescheduleRequestDto rescheduleRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.CreateRescheduleRequest(userId, rescheduleRequestDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return StatusCode(StatusCodes.Status201Created,
                   new ApiResponse<string>(true, "Reschedule request created successfully."));
        }

        [HttpGet("reschedule-requests/{rescheduleRequestId}")]
        public async Task<IActionResult> GetRescheduleRequest(int rescheduleRequestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.GetRescheduleRequestById(userId, rescheduleRequestId);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return Ok(new ApiResponse<RescheduleRequestDetailsDto>(true, null!, result.Value!));
        }

        [HttpGet("to-reschedule-requests")]
        public async Task<IActionResult> GetRescheduleRequestsToProcess()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));
           
            var result = await _requestRepository.GetRequestsToReschedule(userId);
            
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<ToRescheduleRequestListDto>> response = new(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpPut("make-request-in-review/{requestId}")]
        public async Task<IActionResult> MakeRequestInReview(int requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.UpdateRequestStatus(userId, requestId,
                Enums.RequestStatusEnum.InReview,
                "Changed manually until we build the courier entity");

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return NoContent();
        }

        [HttpPut("make-request-approved/{requestId}")]
        public async Task<IActionResult> MakeRequestApproved(int requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.UpdateRequestStatus(userId, requestId,
                Enums.RequestStatusEnum.Approved,
                "Changed manually until we build the courier entity");

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return NoContent();
        }

        [HttpPut("make-request-inprogress/{requestId}")]
        public async Task<IActionResult> MakeRequestInProgress(int requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new ApiResponse<string>(false, "User not authenticated."));

            var result = await _requestRepository.UpdateRequestStatus(userId, requestId,
                Enums.RequestStatusEnum.InProgress,
                "Changed manually until we build the courier entity");

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return NoContent();
        }
    }
}
