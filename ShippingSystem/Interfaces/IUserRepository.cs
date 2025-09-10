using ShippingSystem.DTOs;
using ShippingSystem.Enums;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateUserAsync(ApplicationUser user, string Password);
        Task<OperationResult> AddRoleAsync(ApplicationUser user, RolesEnum role);
        Task<ValueOperationResult<AuthDTO>> LoginUserAsync(LoginDTO loginDTO);
        Task<ValueOperationResult<AuthDTO>> GetUserTokensAsync(ApplicationUser user);
        Task<ValueOperationResult<AuthDTO>> RefreshTokenAsync(string token);
        Task<OperationResult> RevokeTokenAsync(string token);
        Task<bool> IsEmailExistAsync(string email);
    }
}
