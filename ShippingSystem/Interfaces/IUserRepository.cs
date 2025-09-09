using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateUserAsync(ApplicationUser user, string Password);
        Task<OperationResult> AddRoleAsync(ApplicationUser user, RolesEnum role);
        Task<ValueOperationResult<AuthDto>> LoginUserAsync(LoginDto loginDto);
        Task<ValueOperationResult<AuthDto>> GetUserTokensAsync(ApplicationUser user);
        Task<ValueOperationResult<AuthDto>> RefreshTokenAsync(string token);
        Task<OperationResult> RevokeTokenAsync(string token);
        Task<bool> IsEmailExistAsync(string email);
    }
}
