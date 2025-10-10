using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using static ShippingSystem.Helpers.DateTimeExtensions;

namespace ShippingSystem.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShippingSettingsService _shippingSettings;
        private readonly ILogger<ShippingSettingsService> _logger;
        private readonly IUserRepository _userRepository;

        public ShipmentRepository(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IShippingSettingsService shippingSettings,
            ILogger<ShippingSettingsService> logger,
            IUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
            _shippingSettings = shippingSettings;
            _logger = logger;
            _userRepository = userRepository;
        }

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
                var shipment = new Shipment
                {
                    ShipperId = userId,
                    CustomerName = shipmentRequestDTO.CustomerName,
                    CustomerPhone = shipmentRequestDTO.CustomerPhone,
                    CustomerEmail = shipmentRequestDTO.CustomerEmail,
                    CustomerAddress = new Address
                    {
                        Street = shipmentRequestDTO.Street,
                        City = shipmentRequestDTO.City,
                        Governorate = shipmentRequestDTO.Governorate,
                        Details = shipmentRequestDTO.AddressDetails,
                        GoogleMapAddressLink = shipmentRequestDTO.GoogleMapAddressLink
                    },
                    ShipmentDescription = shipmentRequestDTO.ShipmentDescription,
                    ShipmentWeight = shipmentRequestDTO.ShipmentWeight,
                    ShipmentLength = shipmentRequestDTO.ShipmentLength,
                    ShipmentWidth = shipmentRequestDTO.ShipmentWidth,
                    ShipmentHeight = shipmentRequestDTO.ShipmentHeight,
                    Quantity = shipmentRequestDTO.Quantity,
                    ShipmentNotes = shipmentRequestDTO.ShipmentNotes,
                    CashOnDeliveryEnabled = shipmentRequestDTO.CashOnDeliveryEnabled,
                    OpenPackageOnDeliveryEnabled = shipmentRequestDTO.OpenPackageOnDeliveryEnabled,
                    ExpressDeliveryEnabled = shipmentRequestDTO.ExpressDeliveryEnabled,
                    CollectionAmount = shipmentRequestDTO.CollectionAmount,
                    AdditionalWeightCostPrtKg = SettingsConfig.AdditionalWeightCostPrtKg,
                    CollectionFeePercentage = SettingsConfig.CollectionFeePercentage,
                    CollectionFeeThreshold = SettingsConfig.CollectionFeeThreshold
                };

                shipment.CreatedAt = shipment.UpdatedAt = UtcNowTrimmedToSeconds();

                do
                {
                    shipment.ShipmentTrackingNumber =
                        $"SHIP-{shipment.CreatedAt:ddMMyyyy}-{Guid.NewGuid().ToString("N")[..12]}";
                } while (await _context.Shipments.AnyAsync(s => s.ShipmentTrackingNumber == shipment.ShipmentTrackingNumber));

                _context.Shipments.Add(shipment);

                await _context.SaveChangesAsync();

                await UpdateShipmentStatus(userId, shipment.Id, ShipmentStatusEnum.Pending, "Shipment created");

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

            var AllShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(shipment => new ShipmentListDto
                {
                    Id = shipment.Id,
                    CustomerName = shipment.CustomerName,
                    CustomerPhone = shipment.CustomerPhone,
                    CustomerAddress = new AddressDto
                    {
                        Street = shipment.CustomerAddress.Street,
                        City = shipment.CustomerAddress.City,
                        Governorate = shipment.CustomerAddress.Governorate,
                        Details = shipment.CustomerAddress.Details,
                        GoogleMapAddressLink = shipment.CustomerAddress.GoogleMapAddressLink
                    },
                    ShipmentDescription = shipment.ShipmentDescription,
                    ExpressDeliveryEnabled = shipment.ExpressDeliveryEnabled,
                    CollectionAmount = shipment.CollectionAmount,
                    CreatedAt = shipment.CreatedAt,
                    LatestShipmentStatus = shipment.ShipmentStatuses
                        .OrderByDescending(ss => ss.Timestamp)
                        .Select(ss => new LatestShipmentStatusDto
                        {
                            Status = ss.Status,
                            Timestamp = ss.Timestamp,
                        }).FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<ShipmentListDto>>.Ok(AllShipments);
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

            ShipmentDetailsDto ShipmentDetails = new()
            {
                Id = shipment.Id,
                CustomerName = shipment.CustomerName,
                CustomerPhone = shipment.CustomerPhone,
                CustomerAdditionalPhone = shipment.CustomerAdditionalPhone,
                CustomerEmail = shipment.CustomerEmail,
                CustomerAddress = new AddressDto
                {
                    Street = shipment.CustomerAddress.Street,
                    City = shipment.CustomerAddress.City,
                    Governorate = shipment.CustomerAddress.Governorate,
                    Details = shipment.CustomerAddress.Details,
                    GoogleMapAddressLink = shipment.CustomerAddress.GoogleMapAddressLink
                },
                ShipmentDescription = shipment.ShipmentDescription,
                ShipmentWeight = shipment.ShipmentWeight,
                ShipmentLength = shipment.ShipmentLength,
                ShipmentWidth = shipment.ShipmentWidth,
                ShipmentHeight = shipment.ShipmentHeight,
                ShipmentVolume = shipment.ShipmentVolume,
                Quantity = shipment.Quantity,
                ShipmentNotes = shipment.ShipmentNotes,
                CashOnDeliveryEnabled = shipment.CashOnDeliveryEnabled,
                OpenPackageOnDeliveryEnabled = shipment.OpenPackageOnDeliveryEnabled,
                ExpressDeliveryEnabled = shipment.ExpressDeliveryEnabled,
                CollectionAmount = shipment.CollectionAmount,
                ShippingCost = shipment.ShippingCost,
                AdditionalWeight = shipment.AdditionalWeight,
                AdditionalWeightCost = shipment.AdditionalWeightCost,
                CollectionFee = shipment.CollectionFee,
                AdditionalCost = shipment.AdditionalCost,
                TotalCost = shipment.TotalCost,
                NetPayout = shipment.NetPayout,
                CreatedAt = shipment.CreatedAt,
                UpdatedAt = shipment.UpdatedAt,
                ShipmentTrackingNumber = shipment.ShipmentTrackingNumber,
                ShipmentStatuses = [.. shipment.ShipmentStatuses
                    .OrderByDescending(ss => ss.Timestamp)
                    .Select(ss => new ShipmentStatusHistoryDto
                    {
                        Status = ss.Status,
                        Timestamp = ss.Timestamp,
                        Notes = ss.Notes
                    })]
            };

            return ValueOperationResult<ShipmentDetailsDto?>.Ok(ShipmentDetails);
        }

        public async Task<ValueOperationResult<ShipmentDetailsDto?>> UpdateShipment(string userId, int id, UpdateShipmentDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status404NotFound, "Shipment not found");

            shipment.CustomerName = shipmentRequestDTO.CustomerName;
            shipment.CustomerPhone = shipmentRequestDTO.CustomerPhone;
            shipment.CustomerAdditionalPhone = shipmentRequestDTO.CustomerAdditionalPhone;
            shipment.CustomerEmail = shipmentRequestDTO.CustomerEmail;
            shipment.CustomerAddress.Street = shipmentRequestDTO.Street;
            shipment.CustomerAddress.City = shipmentRequestDTO.City;
            shipment.CustomerAddress.Governorate = shipmentRequestDTO.Governorate;
            shipment.CustomerAddress.Details = shipmentRequestDTO.AddressDetails;
            shipment.CustomerAddress.GoogleMapAddressLink = shipmentRequestDTO.GoogleMapAddressLink;
            shipment.ShipmentDescription = shipmentRequestDTO.ShipmentDescription;
            shipment.ShipmentWeight = shipmentRequestDTO.ShipmentWeight;
            shipment.ShipmentLength = shipmentRequestDTO.ShipmentLength;
            shipment.ShipmentWidth = shipmentRequestDTO.ShipmentWidth;
            shipment.ShipmentHeight = shipmentRequestDTO.ShipmentHeight;
            shipment.Quantity = shipmentRequestDTO.Quantity;
            shipment.ShipmentNotes = shipmentRequestDTO.ShipmentNotes;
            shipment.CashOnDeliveryEnabled = shipmentRequestDTO.CashOnDeliveryEnabled;
            shipment.OpenPackageOnDeliveryEnabled = shipmentRequestDTO.OpenPackageOnDeliveryEnabled;
            shipment.ExpressDeliveryEnabled = shipmentRequestDTO.ExpressDeliveryEnabled;
            shipment.CollectionAmount = shipmentRequestDTO.CollectionAmount;

            var hasChanges = _context.Entry(shipment).State == EntityState.Modified
                     || _context.Entry(shipment.CustomerAddress).State == EntityState.Modified;

            if (!hasChanges)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status304NotModified, "No changes detected.");

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
                ShipmentStatusEnum.Returned.ToString(),
            };

            var ToPickupShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(s => new
                {
                    Shipment = s,
                    LatestStatus = s.ShipmentStatuses
                        .OrderByDescending(ss => ss.Timestamp)
                        .Select(ss => new
                        {
                            ss.Status,
                            ss.Timestamp
                        })
                        .FirstOrDefault()
                })
                .Where(ls => validPickupStatuses.Contains(ls.LatestStatus!.Status))
                .Select(s => new ToPickupShipmentListDto
                {
                    Id = s.Shipment.Id,
                    CustomerName = s.Shipment.CustomerName,
                    CustomerPhone = s.Shipment.CustomerPhone,
                    CustomerAddress = new AddressDto
                    {
                        Street = s.Shipment.CustomerAddress.Street,
                        City = s.Shipment.CustomerAddress.City,
                        Governorate = s.Shipment.CustomerAddress.Governorate,
                        Details = s.Shipment.CustomerAddress.Details,
                        GoogleMapAddressLink = s.Shipment.CustomerAddress.GoogleMapAddressLink
                    },
                    ShipmentDescription = s.Shipment.ShipmentDescription,
                    ShipmentWeight = s.Shipment.ShipmentWeight,
                    Quantity = s.Shipment.Quantity,
                    ExpressDeliveryEnabled = s.Shipment.ExpressDeliveryEnabled,
                    CollectionAmount = s.Shipment.CollectionAmount,
                    CreatedAt = s.Shipment.CreatedAt,
                    LatestShipmentStatus = new LatestShipmentStatusDto
                    {
                        Status = s.LatestStatus!.Status,
                        Timestamp = s.LatestStatus.Timestamp
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<ToPickupShipmentListDto>>.Ok(ToPickupShipments);
        }

        public async Task<OperationResult> CreatePickupRequest(string userId, CreatePickupRequestDto pickupRequestDto)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            if (pickupRequestDto.ShipmentIds.Count == 0)
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "No shipments selected for pickup");

            var validShipmentIds = await _context.Shipments
                    .Where(s => s.ShipperId == userId && pickupRequestDto.ShipmentIds.Contains(s.Id))
                    .Select(s => s.Id)
                    .ToListAsync();

            if (validShipmentIds.Count != pickupRequestDto.ShipmentIds.Count)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Some shipments are invalid or not found");

            var pickupRequest = new PickupRequest
            {
                UserId = userId,
                RequestType = RequestTypeEnum.PickupRequest,
                PickupDate = pickupRequestDto.PickupDate,
                WindowStart = pickupRequestDto.WindowStart,
                WindowEnd = pickupRequestDto.WindowEnd,
                PickupAddress = new Address
                {
                    Street = pickupRequestDto.Street,
                    City = pickupRequestDto.City,
                    Governorate = pickupRequestDto.Governorate,
                    Details = pickupRequestDto.AddressDetails,
                    GoogleMapAddressLink = pickupRequestDto.GoogleMapAddressLink
                },
                ContactName = pickupRequestDto.ContactName,
                ContactPhone = pickupRequestDto.ContactPhone,
                ShipmentsCount = validShipmentIds.Count,

                RequestStatus = RequestStatusEnum.Pending,
                PickupRequestShipments = [.. validShipmentIds
                    .Select(id => new PickupRequestShipment
                    {
                        ShipmentId = id
                    })]
            };

            pickupRequest.CreatedAt = pickupRequest.UpdatedAt = UtcNowTrimmedToSeconds();

            await _context.PickupRequests.AddAsync(pickupRequest);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            foreach (var item in validShipmentIds)
                await UpdateShipmentStatus(userId, item, ShipmentStatusEnum.WaitingForPickup, "Ready For Pickup");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<ToReturnShipmentListDto>>> GetShipmentsToReturn(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<ToReturnShipmentListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var toReturnShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(s => new
                {
                    Shipment = s,
                    LatestStatus = s.ShipmentStatuses
                        .OrderByDescending(ss => ss.Timestamp)
                        .FirstOrDefault()
                })
                .Where(ls => ls.LatestStatus!.Status == ShipmentStatusEnum.Delivered.ToString())
                .Select(s => new ToReturnShipmentListDto
                {
                    Id = s.Shipment.Id,
                    CustomerName = s.Shipment.CustomerName,
                    CustomerPhone = s.Shipment.CustomerPhone,
                    CustomerAddress = new AddressDto
                    {
                        Street = s.Shipment.CustomerAddress.Street,
                        City = s.Shipment.CustomerAddress.City,
                        Governorate = s.Shipment.CustomerAddress.Governorate,
                        Details = s.Shipment.CustomerAddress.Details,
                        GoogleMapAddressLink = s.Shipment.CustomerAddress.GoogleMapAddressLink
                    },
                    ShipmentDescription = s.Shipment.ShipmentDescription,
                    ShipmentWeight = s.Shipment.ShipmentWeight,
                    Quantity = s.Shipment.Quantity,
                    ExpressDeliveryEnabled = s.Shipment.ExpressDeliveryEnabled,
                    CollectionAmount = s.Shipment.CollectionAmount,
                    CreatedAt = s.Shipment.CreatedAt,
                    LatestShipmentStatus = new LatestShipmentStatusDto
                    {
                        Status = s.LatestStatus!.Status,
                        Timestamp = s.LatestStatus.Timestamp
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<ToReturnShipmentListDto>>.Ok(toReturnShipments);
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

            var returnRequest = new ReturnRequest
            {
                UserId = userId,
                RequestType = RequestTypeEnum.ReturnRequest,

                ReturnPickupDate = returnRequestDto.ReturnPickupDate,
                ReturnPickupWindowStart = returnRequestDto.ReturnPickupWindowStart,
                ReturnPickupWindowEnd = returnRequestDto.ReturnPickupWindowEnd,
                ReturnPickupAddress = new Address
                {
                    Street = returnRequestDto.CustomerStreet,
                    City = returnRequestDto.CustomerCity,
                    Governorate = returnRequestDto.CustomerGovernorate,
                    Details = returnRequestDto.CustomerAddressDetails,
                    GoogleMapAddressLink = returnRequestDto.CustomerGoogleMapAddressLink
                },
                CustomerContactName = returnRequestDto.CustomerContactName,
                CustomerContactPhone = returnRequestDto.CustomerContactPhone,

                ReturnDate = returnRequestDto.ReturnDate,
                ReturnWindowStart = returnRequestDto.ReturnWindowStart,
                ReturnWindowEnd = returnRequestDto.ReturnWindowEnd,
                ReturnAddress = new Address
                {
                    Street = returnRequestDto.ShipperStreet,
                    City = returnRequestDto.ShipperCity,
                    Governorate = returnRequestDto.ShipperGovernorate,
                    Details = returnRequestDto.ShipperAddressDetails,
                    GoogleMapAddressLink = returnRequestDto.ShipperGoogleMapAddressLink
                },
                ShipperContactName = returnRequestDto.ShipperContactName,
                ShipperContactPhone = returnRequestDto.ShipperContactPhone,


                ShipmentsCount = validShipmentIds.Count,
                RequestStatus = RequestStatusEnum.Pending,

                ReturnRequestShipments = [.. validShipmentIds
                    .Select(id => new ReturnRequestShipment
                    {
                        ShipmentId = id
                    })]
            };

            returnRequest.CreatedAt = returnRequest.UpdatedAt = UtcNowTrimmedToSeconds();

            await _context.ReturnRequests.AddAsync(returnRequest);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            foreach (var item in validShipmentIds)
                await UpdateShipmentStatus(userId, item, ShipmentStatusEnum.WaitingForPickup, "Waiting for pickup to be returned");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<RequestListDto>>> GetAllRequests(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<List<RequestListDto>>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized access");

            var allRequests = await _context.Requests
                .Where(r => r.UserId == userId)
                .Select(r => new RequestListDto
                {
                    Id = r.Id,
                    RequestType = r.RequestType.ToString(),
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    ShipmentsCount = r.ShipmentsCount,
                    Status = r.RequestStatus.ToString()
                })
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<RequestListDto>>.Ok(allRequests);
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
    }
}
