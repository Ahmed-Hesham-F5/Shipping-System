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

        public async Task<OperationResult> AddShipment(string userId, ShipmentFromRequestDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

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
                    ReceiverName = shipmentRequestDTO.ReceiverName,
                    ReceiverPhone = shipmentRequestDTO.ReceiverPhone,
                    ReceiverEmail = shipmentRequestDTO.ReceiverEmail,
                    ReceiverAddress = new ReceiverAddress
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

                await EditShipmentStatus(userId, shipment.Id, ShipmentStatusEnum.Pending, "Shipment created");

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

        public async Task<OperationResult> EditShipmentStatus(string userId, int shipmentId, ShipmentStatusEnum newStatus, string? notes = null)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            Shipment? shipment = await _context.Shipments.FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Shipment not found");

            var getUserRoleResult = await _userRepository.GetUserRoleAsync(userId);

            if (!getUserRoleResult.Success)
                return OperationResult.Fail(getUserRoleResult.StatusCode, getUserRoleResult.ErrorMessage);

            if (getUserRoleResult.Value == RolesEnum.Shipper.ToString() && shipment.ShipperId != userId)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Forbidden");

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
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var AllShipments = await _context.Shipments
                .Where(s => s.ShipperId == userId)
                .Select(shipment => new ShipmentListDto
                {
                    Id = shipment.Id,
                    ReceiverName = shipment.ReceiverName,
                    ReceiverPhone = shipment.ReceiverPhone,
                    ReceiverAddress = new ReceiverAddressDto
                    {
                        Street = shipment.ReceiverAddress.Street,
                        City = shipment.ReceiverAddress.City,
                        Governorate = shipment.ReceiverAddress.Governorate,
                        Details = shipment.ReceiverAddress.Details,
                        GoogleMapAddressLink = shipment.ReceiverAddress.GoogleMapAddressLink
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
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments.Include(s => s.ShipmentStatuses)
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status403Forbidden, "Forbidden");

            ShipmentDetailsDto ShipmentDetails = new ShipmentDetailsDto
            {
                Id = shipment.Id,
                ReceiverName = shipment.ReceiverName,
                ReceiverPhone = shipment.ReceiverPhone,
                ReceiverAdditionalPhone = shipment.ReceiverAdditionalPhone,
                ReceiverEmail = shipment.ReceiverEmail,
                ReceiverAddress = new ReceiverAddressDto
                {
                    Street = shipment.ReceiverAddress.Street,
                    City = shipment.ReceiverAddress.City,
                    Governorate = shipment.ReceiverAddress.Governorate,
                    Details = shipment.ReceiverAddress.Details,
                    GoogleMapAddressLink = shipment.ReceiverAddress.GoogleMapAddressLink
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
                ShipmentStatuses = shipment.ShipmentStatuses
                    .OrderByDescending(ss => ss.Timestamp)
                    .Select(ss => new ShipmentStatusHistoryDto
                    {
                        Status = ss.Status,
                        Timestamp = ss.Timestamp,
                        Notes = ss.Notes
                    }).ToList()
            };

            return ValueOperationResult<ShipmentDetailsDto?>.Ok(ShipmentDetails);
        }

        public async Task<ValueOperationResult<ShipmentDetailsDto?>> UpdateShipment(string userId, int id, ShipmentFromRequestDto shipmentRequestDTO)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return ValueOperationResult<ShipmentDetailsDto?>
                    .Fail(StatusCodes.Status403Forbidden, "Forbidden");

            shipment.ReceiverName = shipmentRequestDTO.ReceiverName;
            shipment.ReceiverPhone = shipmentRequestDTO.ReceiverPhone;
            shipment.ReceiverAdditionalPhone = shipmentRequestDTO.ReceiverAdditionalPhone;
            shipment.ReceiverEmail = shipmentRequestDTO.ReceiverEmail;
            shipment.ReceiverAddress.Street = shipmentRequestDTO.Street;
            shipment.ReceiverAddress.City = shipmentRequestDTO.City;
            shipment.ReceiverAddress.Governorate = shipmentRequestDTO.Governorate;
            shipment.ReceiverAddress.Details = shipmentRequestDTO.AddressDetails;
            shipment.ReceiverAddress.GoogleMapAddressLink = shipmentRequestDTO.GoogleMapAddressLink;
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
                     || _context.Entry(shipment.ReceiverAddress).State == EntityState.Modified;

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
                return OperationResult.Fail(StatusCodes.Status401Unauthorized, "Unauthorized user");

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.ShipperId == userId && s.Id == id);

            if (shipment == null)
                return OperationResult.Fail(StatusCodes.Status403Forbidden, "Forbidden");

            _context.Shipments.Remove(shipment);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }
    }
}
