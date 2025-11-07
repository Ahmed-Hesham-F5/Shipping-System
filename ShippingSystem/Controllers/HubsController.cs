using Microsoft.AspNetCore.Mvc;
using ShippingSystem.DTOs.HubDTOs;
using ShippingSystem.Interfaces;
using ShippingSystem.Responses;

namespace ShippingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubsController(IHubRepository hubRepository) : ControllerBase
    {
        private readonly IHubRepository _hubRepository = hubRepository;

        [HttpPost]
        public async Task<IActionResult> CreateHub([FromBody] CreateHubDto createHubDto)
        {
            var result = await _hubRepository.CreateHubAsync(createHubDto);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<string>(true, "Hub created successfully."));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHubs()
        {
            var result = await _hubRepository.GetAllHubsAsync();
            
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            var response = new ApiResponse<List<HubListDto>>(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }

        [HttpGet("selectable")]
        public async Task<IActionResult> GetSelectableHubs()
        {
            var result = await _hubRepository.GetSelectableHubs();

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    new ApiResponse<string>(false, result.ErrorMessage));

            var response = new ApiResponse<List<HubSelectDto>>(
                success: true,
                message: null!,
                data: result.Value!
            );

            return Ok(response);
        }
    }
}
