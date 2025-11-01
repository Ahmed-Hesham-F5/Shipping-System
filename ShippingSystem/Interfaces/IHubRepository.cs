using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.HubDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IHubRepository
    {
        Task<ValueOperationResult<AuthDTO>> CreateHubAsync(CreateHubDto createHubDto);
        Task<ValueOperationResult<List<HubSelectDto>>> GetSelectableHubs();
    }
}
