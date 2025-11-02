using Microsoft.AspNetCore.Mvc;
using Restaurant.Domain.Services;

namespace Restaurant.WebApi.Controllers;

/// <summary>
/// Controller for accessing Kitchen menu items
/// This demonstrates microservices communication between Restaurant and Kitchen
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IKitchenApiService _kitchenApiService;
    private readonly ILogger<MenuController> _logger;

    public MenuController(IKitchenApiService kitchenApiService, ILogger<MenuController> logger)
    {
        _kitchenApiService = kitchenApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get the full menu from Kitchen API
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenu(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching full menu from Kitchen API");
            var menu = await _kitchenApiService.GetMenuAsync(cancellationToken);
            return Ok(menu);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Kitchen API is unavailable");
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { message = "Kitchen service is currently unavailable" });
        }
    }

    /// <summary>
    /// Get only available menu items from Kitchen API
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAvailableMenu(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching available menu from Kitchen API");
            var menu = await _kitchenApiService.GetAvailableMenuAsync(cancellationToken);
            return Ok(menu);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Kitchen API is unavailable");
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { message = "Kitchen service is currently unavailable" });
        }
    }

    /// <summary>
    /// Get a specific menu item by ID from Kitchen API
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<MenuItemDto>> GetMenuItem(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching menu item {MenuItemId} from Kitchen API", id);
            var menuItem = await _kitchenApiService.GetMenuItemByIdAsync(id, cancellationToken);

            if (menuItem == null)
            {
                return NotFound(new { message = $"Menu item {id} not found" });
            }

            return Ok(menuItem);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Kitchen API is unavailable");
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { message = "Kitchen service is currently unavailable" });
        }
    }
}
