using Microsoft.AspNetCore.Mvc;
using RestaurantMenu.Application.DTOs;
using RestaurantMenu.Application.Services;
using RestaurantMenu.Application.Validators;
using RestaurantMenu.Domain.Enums;
using System.Diagnostics;

namespace RestaurantMenu.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;
    private readonly ILogger<MenuController> _logger;
    private static readonly ActivitySource ActivitySource = new("RestaurantMenu.WebApi", "1.0.0");

    public MenuController(IMenuItemService menuItemService, ILogger<MenuController> logger)
    {
        _menuItemService = menuItemService;
        _logger = logger;
    }

    /// <summary>
    /// Ottiene tutti gli item del menu
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetAllMenuItems");
        activity?.SetTag("operation.type", "query");

        _logger.LogInformation("Fetching all menu items");

        var items = await _menuItemService.GetAllAsync(cancellationToken);
        var itemsList = items.ToList();

        activity?.SetTag("items.count", itemsList.Count);
        _logger.LogInformation("Retrieved {Count} menu items", itemsList.Count);

        return Ok(itemsList);
    }

    /// <summary>
    /// Ottiene un item del menu per ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetMenuItemById");
        activity?.SetTag("menu_item.id", id.ToString());

        _logger.LogInformation("Fetching menu item with ID: {Id}", id);

        var item = await _menuItemService.GetByIdAsync(id, cancellationToken);

        if (item is null)
        {
            _logger.LogWarning("Menu item with ID {Id} not found", id);
            activity?.SetTag("found", false);
            return NotFound(new { message = $"Menu item with ID {id} not found" });
        }

        activity?.SetTag("found", true);
        activity?.SetTag("menu_item.name", item.Name);
        activity?.SetTag("menu_item.category", item.Category.ToString());

        return Ok(item);
    }

    /// <summary>
    /// Ottiene tutti gli item disponibili
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetAvailableMenuItems");

        _logger.LogInformation("Fetching available menu items");

        var items = await _menuItemService.GetAvailableAsync(cancellationToken);
        var itemsList = items.ToList();

        activity?.SetTag("items.count", itemsList.Count);

        return Ok(itemsList);
    }

    /// <summary>
    /// Ottiene gli item per categoria
    /// </summary>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByCategory(MenuCategory category, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetMenuItemsByCategory");
        activity?.SetTag("category", category.ToString());

        _logger.LogInformation("Fetching menu items for category: {Category}", category);

        var items = await _menuItemService.GetByCategoryAsync(category, cancellationToken);
        var itemsList = items.ToList();

        activity?.SetTag("items.count", itemsList.Count);

        return Ok(itemsList);
    }

    /// <summary>
    /// Crea un nuovo item del menu
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("CreateMenuItem");
        activity?.SetTag("menu_item.name", request.Name);
        activity?.SetTag("menu_item.category", request.Category.ToString());

        // Validate request
        var validator = new CreateMenuItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for creating menu item: {Errors}",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            activity?.SetTag("validation.failed", true);
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        _logger.LogInformation("Creating new menu item: {Name}", request.Name);

        var created = await _menuItemService.CreateAsync(request, cancellationToken);

        activity?.SetTag("menu_item.id", created.Id.ToString());
        _logger.LogInformation("Menu item created successfully with ID: {Id}", created.Id);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Aggiorna un item del menu esistente
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMenuItemRequest request, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("UpdateMenuItem");
        activity?.SetTag("menu_item.id", id.ToString());

        // Validate request
        var validator = new UpdateMenuItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for updating menu item {Id}: {Errors}",
                id, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            activity?.SetTag("validation.failed", true);
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        _logger.LogInformation("Updating menu item with ID: {Id}", id);

        var updated = await _menuItemService.UpdateAsync(id, request, cancellationToken);

        if (updated is null)
        {
            _logger.LogWarning("Menu item with ID {Id} not found for update", id);
            activity?.SetTag("found", false);
            return NotFound(new { message = $"Menu item with ID {id} not found" });
        }

        activity?.SetTag("updated", true);
        _logger.LogInformation("Menu item {Id} updated successfully", id);

        return Ok(updated);
    }

    /// <summary>
    /// Elimina un item del menu
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("DeleteMenuItem");
        activity?.SetTag("menu_item.id", id.ToString());

        _logger.LogInformation("Deleting menu item with ID: {Id}", id);

        var deleted = await _menuItemService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            _logger.LogWarning("Menu item with ID {Id} not found for deletion", id);
            activity?.SetTag("found", false);
            return NotFound(new { message = $"Menu item with ID {id} not found" });
        }

        activity?.SetTag("deleted", true);
        _logger.LogInformation("Menu item {Id} deleted successfully", id);

        return NoContent();
    }
}
