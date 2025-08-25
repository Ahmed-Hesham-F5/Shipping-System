using ShippingSystem.DTO;
using ShippingSystem.Responses;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        Task<AuthResponse> AddShipperAsync(ShipperRegisterDto registerDto);
        Task<bool> IsEmailExistAsync(string email);
    }
}
