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
            // Source-generated logging per chiamata microservizio
            MicroserviceLogMessages.CallingMicroservice(_logger, "Kitchen", "GetMenu", traceId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            stopwatch.Stop();

            MicroserviceLogMessages.MicroserviceCallCompleted(
                _logger,
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
            MicroserviceLogMessages.MicroserviceCallFailed(
                _logger,
                ex,
                "Kitchen",
                "GetMenu",
                ex.Message);
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
            MicroserviceLogMessages.CallingMicroservice(_logger, "Kitchen", "GetMenuItemById", traceId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            stopwatch.Stop();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MicroserviceLogMessages.MicroserviceCallCompleted(
                    _logger,
                    "Kitchen",
                    "GetMenuItemById",
                    404,
                    stopwatch.ElapsedMilliseconds);
                return null;
            }

            MicroserviceLogMessages.MicroserviceCallCompleted(
                _logger,
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
            MicroserviceLogMessages.MicroserviceCallFailed(
                _logger,
                ex,
                "Kitchen",
                "GetMenuItemById",
                ex.Message);
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
            MicroserviceLogMessages.CallingMicroservice(_logger, "Kitchen", "GetAvailableMenu", traceId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            stopwatch.Stop();

            MicroserviceLogMessages.MicroserviceCallCompleted(
                _logger,
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
            MicroserviceLogMessages.MicroserviceCallFailed(
                _logger,
                ex,
                "Kitchen",
                "GetAvailableMenu",
                ex.Message);
            throw;
        }
    }
}
