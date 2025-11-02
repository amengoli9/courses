using Microsoft.Extensions.Logging;
using Restaurant.Domain.Services;
using System.Net.Http.Json;

namespace Restaurant.Infrastructure.Services;

public class KitchenApiService : IKitchenApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<KitchenApiService> _logger;

    public KitchenApiService(HttpClient httpClient, ILogger<KitchenApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calling Kitchen API to get full menu");

            var response = await _httpClient.GetAsync("/api/menu", cancellationToken);
            response.EnsureSuccessStatusCode();

            var menu = await response.Content.ReadFromJsonAsync<IEnumerable<MenuItemDto>>(cancellationToken);

            _logger.LogInformation("Retrieved {Count} menu items from Kitchen API", menu?.Count() ?? 0);

            return menu ?? Enumerable.Empty<MenuItemDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling Kitchen API to get menu");
            throw;
        }
    }

    public async Task<MenuItemDto?> GetMenuItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calling Kitchen API to get menu item {MenuItemId}", id);

            var response = await _httpClient.GetAsync($"/api/menu/{id}", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Menu item {MenuItemId} not found in Kitchen API", id);
                return null;
            }

            response.EnsureSuccessStatusCode();

            var menuItem = await response.Content.ReadFromJsonAsync<MenuItemDto>(cancellationToken);

            _logger.LogInformation("Retrieved menu item {MenuItemId} from Kitchen API", id);

            return menuItem;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling Kitchen API to get menu item {MenuItemId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<MenuItemDto>> GetAvailableMenuAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calling Kitchen API to get available menu");

            var response = await _httpClient.GetAsync("/api/menu/available", cancellationToken);
            response.EnsureSuccessStatusCode();

            var menu = await response.Content.ReadFromJsonAsync<IEnumerable<MenuItemDto>>(cancellationToken);

            _logger.LogInformation("Retrieved {Count} available menu items from Kitchen API", menu?.Count() ?? 0);

            return menu ?? Enumerable.Empty<MenuItemDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling Kitchen API to get available menu");
            throw;
        }
    }
}
