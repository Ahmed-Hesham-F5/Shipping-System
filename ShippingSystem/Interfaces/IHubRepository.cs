using ShippingSystem.DTOs.HubDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IHubRepository
    {
        Task<OperationResult> CreateHubAsync(CreateHubDto createHubDto);
        Task<ValueOperationResult<List<HubListDto>>> GetAllHubsAsync();
        Task<ValueOperationResult<List<HubSelectDto>>> GetSelectableHubs();
        Task<OperationResult> AddEmployeeToHubAsync(int hubId, AssignEmployeeDto assignEmployeeDto);
    }
}
