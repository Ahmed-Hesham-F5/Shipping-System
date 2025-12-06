using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.HubDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Results;
using static ShippingSystem.Helpers.DateTimeExtensions;

namespace ShippingSystem.Repositories
{
    public class HubRepository(AppDbContext context, IMapper mapper)
        : IHubRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationResult> CreateHubAsync(CreateHubDto createHubDto)
        {
            Hub hub = new Hub();
            if (Enum.TryParse<HubTypesEnum>(createHubDto.Type, true, out var hubType))
                hub.Type = hubType;
            else
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "Invalid hub type");
         
            hub = _mapper.Map(createHubDto, hub);

            hub.CreatedAt = hub.UpdatedAt= UtcNowTrimmedToSeconds();

            await _context.Hubs.AddAsync(hub);
            var result = await _context.SaveChangesAsync();

            if (result < 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public Task<ValueOperationResult<List<HubListDto>>> GetAllHubsAsync()
        {
            var hubs = _context.Hubs
                .ProjectTo<HubListDto>(_mapper.ConfigurationProvider)
                .ToList();

            return Task.FromResult(ValueOperationResult<List<HubListDto>>.Ok(hubs));
        }

        public Task<ValueOperationResult<List<HubSelectDto>>> GetSelectableHubs()
        {
            var hubs = _context.Hubs
                .ProjectTo<HubSelectDto>(_mapper.ConfigurationProvider)
                .ToList();

            return Task.FromResult(ValueOperationResult<List<HubSelectDto>>.Ok(hubs));
        }

        public async Task<OperationResult> AddEmployeeToHubAsync(int hubId, AssignEmployeeDto assignEmployeeDto)
        {
            var hub = await _context.Hubs.FindAsync(hubId);
            if (hub == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Hub not found");

            var employee = await _context.Employees.FindAsync(assignEmployeeDto.EmployeeId);
            if (employee == null)
                return OperationResult.Fail(StatusCodes.Status404NotFound, "Employee not found");

            hub.Employees ??= new List<Employee>();
            if (hub.Employees.Any(e => e.EmployeeId == assignEmployeeDto.EmployeeId))
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "Employee already assigned to this hub");

            hub.Employees.Add(employee);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return OperationResult.Fail(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");

            return OperationResult.Ok();
        }

        public async Task<ValueOperationResult<List<GovernorateListDto>>> GetGovernoratesAsync()
        {
            var governorates = await _context.Governorates
                .ProjectTo<GovernorateListDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return ValueOperationResult<List<GovernorateListDto>>.Ok(governorates);
        }

        public async Task<ValueOperationResult<HubProfileDto>> GetHubProfileAsync(int hubId)
        {
            var hub = await _context.Hubs
                .Where(h => h.Id == hubId)
                .ProjectTo<HubProfileDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(); 

            if (hub == null)
                return ValueOperationResult<HubProfileDto>.Fail(StatusCodes.Status404NotFound, "Hub not found");

            var hubProfileDto = _mapper.Map<HubProfileDto>(hub);
            return ValueOperationResult<HubProfileDto>.Ok(hubProfileDto);
        }
    }
}
