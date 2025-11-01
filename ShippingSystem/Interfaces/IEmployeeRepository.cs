using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.EmployeeDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<ValueOperationResult<AuthDTO>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<ValueOperationResult<List<string>>> GetAssignableEmployeeRoles();
    }
}
