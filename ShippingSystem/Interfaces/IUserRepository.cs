using ShippingSystem.DTO;
using ShippingSystem.Enums;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IUserRepository
    {
        public Task<OperationResult> CreateUserAsync(ApplicationUser user, string Password);
        public Task<OperationResult> AddRoleAsync(ApplicationUser user, RolesEnum role);
        public Task<AuthResponse> LoginUserAsync(LoginDto loginDto);
        public Task<AuthResponse> GetUserTokensAsync(ApplicationUser user);
        public Task<AuthResponse> RefreshTokenAsync(string token);
        public Task<bool> RevokeTokenAsync(string token);

        Task<bool> IsEmailExistAsync(string email);
    }
}
