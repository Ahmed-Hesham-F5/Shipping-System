using ShippingSystem.DTO;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        public Task<bool> AddShipper(RegisterDto registerDto);
        public Task<bool> IsEmailExist(string email);
    }
}
