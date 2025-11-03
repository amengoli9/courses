using Microsoft.Extensions.Logging;
using Restaurant.Domain.Services;
using System.Net.Http.Json;
using System.Diagnostics;
using Common.Logging;

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
        var url = "/api/menu";
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace";
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Extension method source-generated su ILogger
            _logger.CallingMicroservice("Kitchen", "GetMenu", traceId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            stopwatch.Stop();

            _logger.MicroserviceCallCompleted(
                "Kitchen",
                "GetMenu",
                (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds);

            response.EnsureSuccessStatusCode();

            var menu = await response.Content.ReadFromJsonAsync<IEnumerable<MenuItemDto>>(cancellationToken);

            return menu ?? Enumerable.Empty<MenuItemDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.MicroserviceCallFailed(ex, "Kitchen", "GetMenu", ex.Message);
            throw;
        }
    }

    public async Task<MenuItemDto?> GetMenuItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var url = $"/api/menu/{id}";
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace";
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.CallingMicroservice("Kitchen", "GetMenuItemById", traceId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            stopwatch.Stop();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.MicroserviceCallCompleted(
                    "Kitchen",
                    "GetMenuItemById",
                    404,
                    stopwatch.ElapsedMilliseconds);
                return null;
            }

            _logger.MicroserviceCallCompleted(
                "Kitchen",
                "GetMenuItemById",
                (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds);

            response.EnsureSuccessStatusCode();

            var menuItem = await response.Content.ReadFromJsonAsync<MenuItemDto>(cancellationToken);

            return menuItem;
        }
        catch (HttpRequestException ex)
        {
            _logger.MicroserviceCallFailed(ex, "Kitchen", "GetMenuItemById", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<MenuItemDto>> GetAvailableMenuAsync(CancellationToken cancellationToken = default)
    {
        var url = "/api/menu/available";
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace";
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.CallingMicroservice("Kitchen", "GetAvailableMenu", traceId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            stopwatch.Stop();

            _logger.MicroserviceCallCompleted(
                "Kitchen",
                "GetAvailableMenu",
                (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds);

            response.EnsureSuccessStatusCode();

            var menu = await response.Content.ReadFromJsonAsync<IEnumerable<MenuItemDto>>(cancellationToken);

            return menu ?? Enumerable.Empty<MenuItemDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.MicroserviceCallFailed(ex, "Kitchen", "GetAvailableMenu", ex.Message);
            throw;
        }
    }
}
