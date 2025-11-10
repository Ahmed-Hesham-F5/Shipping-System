using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.AuthenticationDTOs;
using ShippingSystem.Enums;
using ShippingSystem.Models;
using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateUserAsync(ApplicationUser user, string Password);
        Task<OperationResult> AddRoleAsync(ApplicationUser user, RolesEnum role);
        Task<ValueOperationResult<AuthDTO>> LoginUserAsync(LoginDto loginDTO);
        Task<ValueOperationResult<AuthDTO>> GetUserTokensAsync(ApplicationUser user);
        Task<ValueOperationResult<AuthDTO>> RefreshTokenAsync(string token);
        Task<OperationResult> RevokeRefreshTokenAsync(string token);
        Task<OperationResult> RevokeAccessTokenAsync(string userId);
        Task<bool> IsEmailExistAsync(string email);
        Task<ValueOperationResult<string>> GetUserRoleAsync(string userId);
        Task<ValueOperationResult<AddressDto?>> GetUserAddressAsync(string userEmail);
        Task<ValueOperationResult<ForgetPasswordResponseDto>> RequestForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto);
        Task<OperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<OperationResult> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<OperationResult> FirstLoginChangePasswordAsync(FirstLoginChangePasswordDto firstLoginChangechangePasswordDto);
    }
}
