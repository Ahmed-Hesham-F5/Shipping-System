using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShippingSystem.Data;
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
            var hub = _mapper.Map<Hub>(createHubDto);

            if (Enum.TryParse<HubTypesEnum>(createHubDto.Type, true, out var hubType))
                hub.Type = hubType;
            else
                return OperationResult.Fail(StatusCodes.Status400BadRequest, "Invalid hub type");

            hub.CreatedAt = UtcNowTrimmedToSeconds();

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
    }
}
