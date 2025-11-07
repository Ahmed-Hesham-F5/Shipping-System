using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.EmployeeDTOs;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<OperationResult> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        ValueOperationResult<List<string>> GetAssignableEmployeeRoles();
        Task<ValueOperationResult<List<EmployeeListDto>>> GetAllEmployeesAsync();
    }
}
