using ShippingSystem.DTO;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        Task<bool> AddShipperAsync(RegisterDto registerDto);
        Task<bool> IsEmailExistAsync(string email);
    }
}
