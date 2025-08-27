using ShippingSystem.DTO;
using ShippingSystem.Responses;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        public Task<ValueOperationResult<AuthResponse>> AddShipperAsync(ShipperRegisterDto ShipperRegisterDto);
    }
}
