using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.PhoneNumberDTOs;
using ShippingSystem.DTOs.ShipperDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        Task<OperationResult> CreateShipperAsync(CreateShipperDto createShipperDto);
        Task<ValueOperationResult<ShipperProfileDto>> GetShipperProfileAsync(string shipperId);
        Task<OperationResult> AddShipperAddressAsync(string shipperId, AddressDto addressDto);
        Task<OperationResult> UpdateShipperAddressAsync(string shipperId, int addressId, AddressDto addressDto);
        Task<OperationResult> DeleteShipperAddressAsync(string shipperId, int addressId);
        Task<OperationResult> AddPhoneNumberAsync(string shipperId, PhoneNumberDto phoneNumberDto);
        Task<OperationResult> DeletePhoneNumberAsync(string shipperId, PhoneNumberDto phoneNumberDto);
    }
}
