using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.EmailDTOs;
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
        Task<OperationResult> ChangeEmailRequestAsync(string shipperId, ChangeEmailRequestDto changeEmailRequestDto);
        Task<OperationResult> ChangeEmailAsync(ChangeEmailDto emailDto);
        Task<OperationResult> UpdateCompanyInformationAsync(string shipperId, UpdateCompanyInfoDto updateCompanyInfoDto);
        Task<OperationResult> UpdateShipperNameAsync(string shipperId, UpdateShipperNameDto updateShipperNameDto);
        Task<ValueOperationResult<List<AddressDto>>> GetShipperAddressesAsync(string shipperId);
    }
}
