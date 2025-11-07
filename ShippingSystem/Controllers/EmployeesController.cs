using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.EmployeeDTOs;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(IEmployeeRepository employeeRepository) : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto employee)
        {
            var result = await _employeeRepository.CreateEmployeeAsync(employee);
            
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));
            
            ApiResponse<string> response = new(
                success: true,
                message: "Employee created successfully!",
                data: null
            );

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await _employeeRepository.GetAllEmployeesAsync();

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<EmployeeListDto>> response = new(
                success: true,
                message: null!,
                data: result.Value
            );

            return Ok(response);
        }

        [HttpGet("assignable-roles")]
        public IActionResult GetAssignableEmployeeRoles()
        {
            var result = _employeeRepository.GetAssignableEmployeeRoles();

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            ApiResponse<List<string>> response = new(
                success: true,
                message: "User registered successfully!",
                data: result.Value
            );

            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}
