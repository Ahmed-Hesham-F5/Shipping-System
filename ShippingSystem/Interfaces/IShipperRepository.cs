using ShippingSystem.DTO;
using ShippingSystem.Models;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        public Task<bool> AddUser(RegisterDto registerDto);
        public Task<bool> IsEmailExist(string email);
    }
}
