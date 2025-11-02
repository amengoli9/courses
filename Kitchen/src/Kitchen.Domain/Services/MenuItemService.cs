using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;
using Kitchen.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Common.Logging;
using System.Diagnostics;

namespace Kitchen.Domain.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repository;
    private readonly ILogger<MenuItemService> _logger;

    public MenuItemService(IMenuItemRepository repository, ILogger<MenuItemService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByCategoryAsync(category, cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAvailableAsync(cancellationToken);
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        var entityId = menuItem.Id.ToString();
        var stopwatch = Stopwatch.StartNew();

        _logger.EntityCreating("MenuItem", entityId);
        _logger.ServiceOperationStarted("MenuItemService", "CreateAsync");

        try
        {
            var result = await _repository.AddAsync(menuItem, cancellationToken);
            stopwatch.Stop();

            _logger.EntityCreated("MenuItem", entityId);
            _logger.ServiceOperationCompleted("MenuItemService", "CreateAsync", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            _logger.ServiceOperationFailed(ex, "MenuItemService", "CreateAsync");
            throw;
        }
    }

    public async Task<MenuItem?> UpdateAsync(Guid id, MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        var entityId = id.ToString();
        var stopwatch = Stopwatch.StartNew();

        _logger.EntityUpdating("MenuItem", entityId);

        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            _logger.EntityNotFound("MenuItem", entityId);
            return null;
        }

        try
        {
            existing.Name = menuItem.Name;
            existing.Description = menuItem.Description;
            existing.Price = menuItem.Price;
            existing.Category = menuItem.Category;
            existing.IsAvailable = menuItem.IsAvailable;
            existing.Allergens = menuItem.Allergens;
            existing.ImageUrl = menuItem.ImageUrl;
            existing.PreparationTimeMinutes = menuItem.PreparationTimeMinutes;
            existing.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(existing, cancellationToken);
            stopwatch.Stop();

            _logger.EntityUpdated("MenuItem", entityId);
            _logger.ServiceOperationCompleted("MenuItemService", "UpdateAsync", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            _logger.ServiceOperationFailed(ex, "MenuItemService", "UpdateAsync");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entityId = id.ToString();
        var stopwatch = Stopwatch.StartNew();

        _logger.EntityDeleting("MenuItem", entityId);

        try
        {
            var result = await _repository.DeleteAsync(id, cancellationToken);
            stopwatch.Stop();

            if (result)
            {
                _logger.EntityDeleted("MenuItem", entityId);
                _logger.ServiceOperationCompleted("MenuItemService", "DeleteAsync", stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.EntityNotFound("MenuItem", entityId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.ServiceOperationFailed(ex, "MenuItemService", "DeleteAsync");
            throw;
        }
    }
}
