using ShippingSystem.DTO;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IShipperRepository
    {
        public Task<ValueOperationResult<AuthDto>> AddShipperAsync(ShipperRegisterDto ShipperRegisterDto);
    }
}
