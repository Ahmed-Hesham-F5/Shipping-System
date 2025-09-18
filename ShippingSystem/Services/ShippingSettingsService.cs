using Microsoft.EntityFrameworkCore;
using ShippingSystem.Data;
using ShippingSystem.Enums;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using static ShippingSystem.Helpers.DateTimeExtensions;

public class ShippingSettingsService : IShippingSettingsService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ShippingSettingsService> _logger;

    // Cached configuration (lives until explicitly refreshed)
    private ShippingConfig? _cachedConfig;

    // Prevents concurrent reload attempts
    private readonly SemaphoreSlim _lock = new(1, 1);

    public ShippingSettingsService(
            IServiceScopeFactory scopeFactory,
            ILogger<ShippingSettingsService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task<ShippingConfig> GetConfigAsync()
    {
        if (_cachedConfig != null)
            return _cachedConfig;

        await LoadConfigAsync();
        return _cachedConfig!;
    }

    public Task RefreshAsync() => LoadConfigAsync(force: true);

    private async Task LoadConfigAsync(bool force = false)
    {
        await _lock.WaitAsync();
        try
        {
            if (!force && _cachedConfig != null)
                return;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Load settings from database
            var settings = await db.ShippingSettings.ToListAsync();

            // Map database records into strongly-typed config
            _cachedConfig = new ShippingConfig
            {
                AdditionalWeightCostPrtKg = decimal.Parse(
                    settings.First(s => s.Key == ShippingSettingKeys.AdditionalWeightCostPrtKg).Value),
                CollectionFeePercentage = decimal.Parse(
                    settings.First(s => s.Key == ShippingSettingKeys.CollectionFeePercentage).Value),
                CollectionFeeThreshold = decimal.Parse(
                    settings.First(s => s.Key == ShippingSettingKeys.CollectionFeeThreshold).Value)
            };

            _logger.LogInformation("Shipping settings loaded at {LoadedAt}", UtcNowTrimmedToSeconds());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load shipping settings");
            throw;
        }
        finally
        {
            _lock.Release();
        }
    }
}
