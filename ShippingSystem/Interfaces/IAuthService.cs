using ShippingSystem.DTO;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using System.IdentityModel.Tokens.Jwt;

namespace ShippingSystem.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginDto loginDto);
    }
}
