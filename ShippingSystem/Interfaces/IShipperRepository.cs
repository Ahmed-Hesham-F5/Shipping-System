using ShippingSystem.DTO;
using ShippingSystem.Models;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        public Task<bool> adduser(RegisterDto registerDto);
    }
}
