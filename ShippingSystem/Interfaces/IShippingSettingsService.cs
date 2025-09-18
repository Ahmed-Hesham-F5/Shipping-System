using ShippingSystem.Models;

namespace ShippingSystem.Interfaces
{
    public interface IShippingSettingsService
    {
        Task<ShippingConfig> GetConfigAsync();
        Task RefreshAsync(); // Force reload (after admin updates)
    }
}
