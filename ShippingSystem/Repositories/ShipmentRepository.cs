using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.RequestDTOs;
using ShippingSystem.DTOs.ShipmentDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using static ShippingSystem.Helpers.DateTimeExtensions;

namespace ShippingSystem.Repositories
{
    public class ShipmentRepository(AppDbContext context,
        UserManager<ApplicationUser> userManager,
        IShippingSettingsService shippingSettings,
        ILogger<ShippingSettingsService> logger,
        IUserRepository userRepository,
        IMapper mapper) : IShipmentRepository
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IShippingSettingsService _shippingSettings = shippingSettings;
        private readonly ILogger<ShippingSettingsService> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationResult> AddShipment(string userId, CreateShipmentDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var SettingsConfig = await _shippingSettings.GetConfigAsync();

            if (SettingsConfig == null)
            {
                _logger.LogError("Shipping settings configuration is missing.");

                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var shipment = _mapper.Map<Shipment>(shipmentRequestDTO);
                shipment.ShipperId = userId;
                shipment.CreatedAt = shipment.UpdatedAt = UtcNowTrimmedToSeconds();

                do
                {
                    shipment.ShipmentTrackingNumber =
                        $"SHIP-{shipment.CreatedAt:ddMMyyyy}-{Guid.NewGuid().ToString("N")[..12]}";
                } while (await _context.Shipments.AnyAsync(s => s.ShipmentTrackingNumber == shipment.ShipmentTrackingNumber));

                _context.Shipments.Add(shipment);

                await _context.SaveChangesAsync();

                OperationResult UpdateStatusResult;
                
                if (shipmentRequestDTO.IsDelivered)
                {
                    UpdateStatusResult = await UpdateShipmentStatus(userId, shipment.Id, ShipmentStatusEnum.Delivered, "Shipment Delivered Successfully");
                }
                else
                {
                    UpdateStatusResult = await UpdateShipmentStatus(userId, shipment.Id, ShipmentStatusEnum.Pending, "Shipment Created");
                }

                if (!UpdateStatusResult.Success)
                {
                    await transaction.RollbackAsync();
                    return UpdateStatusResult;
                }

                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException?.Message.Contains("IX_Shipment_TrackingNumber") == true)
            {
                await transaction.RollbackAsync();

                _logger.LogError(
                    "Could not generate unique tracking number for user {UserId}.",
                    userId
                );

                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex, "Failed to add shipment for user {UserId}", userId);

                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }

            return OperationResult.Ok();
        }

        public async Task<OperationResult> UpdateShipmentStatus(string userId, int shipmentId, ShipmentStatusEnum newStatus, string? notes = null)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            Shipment? shipment = await _context.Shipments.FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipment not found");

            var getUserRoleResult = await _userRepository.GetUserRoleAsync(userId);

            if (!getUserRoleResult.Success)
                return OperationResult.Fail(getUserRoleResult.StatusCode, getUserRoleResult.ErrorMessage);

            if (getUserRoleResult.Value == RolesEnum.Shipper.ToString() && shipment.ShipperId != userId)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipment not found");

            var latestStatus = shipment.ShipmentStatuses
                .OrderByDescending(ss => ss.Timestamp)
                .FirstOrDefault();

            if (latestStatus?.Status == newStatus.ToString())
                return OperationResult.Fail(StatusCodes.Status400BadRequest,
                    "The shipment already has the specified status.");

            var shipmentStatus = new ShipmentStatus(shipmentId, newStatus.ToString(), notes);

            _context.ShipmentStatuses.Add(shipmentStatus);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<ShipmentListDto>>> GetAllShipments(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<ShipmentListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var allShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .ProjectTo<ShipmentListDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<ShipmentListDto>>.Ok(allShipments);
        }

        public async Task<ValueOperationResult<ShipmentDetailsDto?>> GetShipmentById(string userId, int id)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var shipment = await _context.Shipments.Include(s => s.ShipmentStatuses)
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status404NotFound, "Shipment not found");

            ShipmentDetailsDto ShipmentDetails = _mapper.Map<ShipmentDetailsDto>(shipment);

            return ValueOperationResult<ShipmentDetailsDto?>.Ok(ShipmentDetails);
        }

        public async Task<ValueOperationResult<ShipmentDetailsDto?>> UpdateShipment(string userId, int id, UpdateShipmentDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var shipment = await _context.Shipments
                .Include(s => s.ShipmentStatuses)
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status404NotFound, "Shipment not found");

            if (shipment.ShipmentStatuses
                .OrderByDescending(ss => ss.Timestamp)
                .FirstOrDefault()?.Status != ShipmentStatusEnum.Pending.ToString())
            {
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status409Conflict,
                        "Update is only allowed for pending shipments.");
            }

            _mapper.Map(shipmentRequestDTO, shipment);

            if (!_context.ChangeTracker.HasChanges())
                return ValueOperationResult<ShipmentDetailsDto?>.Ok(null);

            shipment.UpdatedAt = UtcNowTrimmedToSeconds();

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred. Please try again later.");

            return await GetShipmentById(userId, id);
        }

        public async Task<OperationResult> DeleteShipment(string userId, int id)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var shipment = await _context.Shipments
                .Include(s => s.ShipmentStatuses)
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipment not found");

            var latestStatus = shipment.ShipmentStatuses
                .OrderByDescending(ss => ss.Timestamp)
                .FirstOrDefault()?.Status;

            if (latestStatus != ShipmentStatusEnum.Pending.ToString())
                return OperationResult.Fail(StatusCodes.Status409Conflict,
                    "Deletion is only allowed for pending shipments.");

            _context.Shipments.Remove(shipment);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<ToPickupShipmentListDto>>> GetShipmentsToPickup(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<ToPickupShipmentListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var validPickupStatuses = new List<string>
            {
                ShipmentStatusEnum.Pending.ToString(),
                ShipmentStatusEnum.Returned.ToString()
            };

            var toPickupShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .ProjectTo<ToPickupShipmentListDto>(_mapper.ConfigurationProvider)
                .Where(s => validPickupStatuses.Contains(s.LatestShipmentStatus!.Status))
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<ToPickupShipmentListDto>>.Ok(toPickupShipments);
        }

        public async Task<ValueOperationResult<List<ToReturnShipmentListDto>>> GetShipmentsToReturn(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<ToReturnShipmentListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var toReturnShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .ProjectTo<ToReturnShipmentListDto>(_mapper.ConfigurationProvider)
                .Where(s => s.LatestShipmentStatus!.Status == ShipmentStatusEnum.Delivered.ToString())
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<ToReturnShipmentListDto>>.Ok(toReturnShipments);
        }

        public async Task<ValueOperationResult<ShipmentStatusStatisticsDto>> GetShipmentStatusStatistics(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ShipmentStatusStatisticsDto>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var statistics = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(s => s.ShipmentStatuses
                    .OrderByDescending(ss => ss.Timestamp)
                    .FirstOrDefault()!.Status)
                .GroupBy(status => status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .AsNoTracking()
                .ToListAsync();

            var result = new ShipmentStatusStatisticsDto
            {
                PendingShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.Pending.ToString())?.Count ?? 0,
                WaitingForPickupShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.WaitingForPickup.ToString())?.Count ?? 0,
                PickedUpShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.PickedUp.ToString())?.Count ?? 0,
                InWarehouseShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.InWarehouse.ToString())?.Count ?? 0,
                OnHoldShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.OnHold.ToString())?.Count ?? 0,
                OutForDeliveryShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.OutForDelivery.ToString())?.Count ?? 0,
                FailedDeliveryShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.FailedDelivery.ToString())?.Count ?? 0,
                ReturningToWarehouseShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.ReturningToWarehouse.ToString())?.Count ?? 0,
                ReturningToShipperShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.ReturningToShipper.ToString())?.Count ?? 0,
                DeliveredShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.Delivered.ToString())?.Count ?? 0,
                ReturnedShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.Returned.ToString())?.Count ?? 0,
                LostShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.Lost.ToString())?.Count ?? 0,
                DamagedShipmentsCount = statistics.FirstOrDefault(s => s.Status == ShipmentStatusEnum.Damaged.ToString())?.Count ?? 0,
            };

            return ValueOperationResult<ShipmentStatusStatisticsDto>.Ok(result);
        }

        public async Task<ValueOperationResult<List<ToCancelShipmentListDto>>> GetShipmentsToCancel(string userId)
        {
            try
            {
                if (await _userManager.FindByIdAsync(userId) == null)
                    return ValueOperationResult<List<ToCancelShipmentListDto>>
                        .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

                var validRequestCancelStatuses = new List<RequestStatusEnum>
                {
                    RequestStatusEnum.InReview,
                    RequestStatusEnum.Approved,
                    RequestStatusEnum.InProgress
                };

                var validShipmentCancelStatuses = new List<string>
                {
                    ShipmentStatusEnum.InReviewForPickup.ToString(),
                    ShipmentStatusEnum.WaitingForPickup.ToString(),
                    ShipmentStatusEnum.InReviewForReturn.ToString(),
                    ShipmentStatusEnum.WaitingForReturn.ToString(),
                    ShipmentStatusEnum.InReviewForExchange.ToString(),
                    ShipmentStatusEnum.WaitingForExchange.ToString(),
                    ShipmentStatusEnum.InReviewForDelivery.ToString(),
                    ShipmentStatusEnum.WaitingForDelivery.ToString(),
                    ShipmentStatusEnum.OutForDelivery.ToString()
                };

                List<ToCancelShipmentListDto> toCancelShipments = await _context.PickupRequestShipments
                    .Where(ps => ps.PickupRequest.UserId == userId)
                    .Where(ps => validRequestCancelStatuses.Contains(ps.PickupRequest.RequestStatus))
                    .Where(ps => validShipmentCancelStatuses.Contains(ps.Shipment.ShipmentStatuses
                        .OrderByDescending(ss => ss.Timestamp)
                        .Select(ss => ss.Status)
                        .FirstOrDefault()!))
                    .ProjectTo<ToCancelShipmentListDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync();

                toCancelShipments.AddRange(
                    await _context.ReturnRequestShipments
                    .Where(ps => ps.ReturnRequest.UserId == userId)
                    .Where(ps => validRequestCancelStatuses.Contains(ps.ReturnRequest.RequestStatus))
                    .Where(ps => validShipmentCancelStatuses.Contains(ps.Shipment.ShipmentStatuses
                        .OrderByDescending(ss => ss.Timestamp)
                        .Select(ss => ss.Status)
                        .FirstOrDefault()!))
                    .ProjectTo<ToCancelShipmentListDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync()
                );

                return ValueOperationResult<List<ToCancelShipmentListDto>>.Ok(toCancelShipments);
            }
            catch
            {
                return ValueOperationResult<List<ToCancelShipmentListDto>>.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
