using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.ShipperDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        Task<ValueOperationResult<AuthDTO>> CreateShipperAsync(CreateShipperDto createShipperDto);
        Task<ValueOperationResult<ShipperAddressDto?>> GetShipperAddressAsync(string shipperUserEmail);
    }
}
