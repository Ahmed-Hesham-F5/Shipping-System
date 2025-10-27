using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        Task<ValueOperationResult<AuthDTO>> AddShipperAsync(ShipperRegisterDTO shipperRegisterDTO);
        Task<ValueOperationResult<ShipperAddressDto?>> GetShipperAddressAsync(string shipperUserEmail);
    }
}
