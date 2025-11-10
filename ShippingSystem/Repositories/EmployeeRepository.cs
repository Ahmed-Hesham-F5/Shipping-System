using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.DTOs.EmployeeDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using static ShippingSystem.Helpers.DateTimeExtensions;

namespace ShippingSystem.Repositories
{
    public class EmployeeRepository(AppDbContext context,
        IUserRepository userRepository,
        IMapper mapper
        ) : IEmployeeRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationResult> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                if (!Enum.TryParse(createEmployeeDto.Role, out RolesEnum Role))
                    return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status400BadRequest, "Invalid role specified.");


                var user = new ApplicationUser
                {
                    UserName = createEmployeeDto.Email,
                    Email = createEmployeeDto.Email,
                    FirstName = createEmployeeDto.FirstName,
                    LastName = createEmployeeDto.LastName,
                    Role = Role,
                    MustChangePassword = true
                };

                user.Phones?.Add(new UserPhone
                {
                    PhoneNumber = createEmployeeDto.PhoneNumber,
                    User = user,
                    UserId = user.Id
                });

                var CreateUserResult = await _userRepository.CreateUserAsync(user, createEmployeeDto.PhoneNumber);

                if (!CreateUserResult.Success)
                    return ValueOperationResult<AuthDTO>.Fail(CreateUserResult.StatusCode, CreateUserResult.ErrorMessage);

                var addRoleResult = await _userRepository.AddRoleAsync(user, Role);

                if (!addRoleResult.Success)
                    return ValueOperationResult<AuthDTO>.Fail(addRoleResult.StatusCode, addRoleResult.ErrorMessage);

                var employee = new Employee
                {
                    EmployeeId = user.Id,
                    HubId = createEmployeeDto.HubId
                };

                employee.CreatedAt = UtcNowTrimmedToSeconds();

                _context.Employees.Add(employee);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                    return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");

                await transaction.CommitAsync();

                return OperationResult.Ok();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e.Message.ToString());
                return ValueOperationResult<AuthDTO>.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ValueOperationResult<List<EmployeeListDto>>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees
                .ProjectTo<EmployeeListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return ValueOperationResult<List<EmployeeListDto>>.Ok(employees);
        }

        public ValueOperationResult<List<string>> GetAssignableEmployeeRoles()
        {
            var roles = new List<string>
            {
                RolesEnum.HubManager.ToString(),
                RolesEnum.Courier.ToString(),
                RolesEnum.Storekeeper.ToString(),
                RolesEnum.OperationsAgent.ToString(),
                RolesEnum.Accountant.ToString()
            };

            return ValueOperationResult<List<string>>.Ok(roles);
        }
    }
}
