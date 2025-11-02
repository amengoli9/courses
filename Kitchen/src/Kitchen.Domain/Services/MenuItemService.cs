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

        BusinessLogMessages.CreatingEntity(_logger, "MenuItem", entityId);
        BusinessLogMessages.ServiceOperationStarted(_logger, "MenuItemService", "CreateAsync");

        try
        {
            var result = await _repository.AddAsync(menuItem, cancellationToken);
            stopwatch.Stop();

            BusinessLogMessages.EntityCreated(_logger, "MenuItem", entityId);
            BusinessLogMessages.ServiceOperationCompleted(_logger, "MenuItemService", "CreateAsync", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            BusinessLogMessages.BusinessOperationFailed(_logger, ex, "CreateAsync", "MenuItem", entityId);
            throw;
        }
    }

    public async Task<MenuItem?> UpdateAsync(Guid id, MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        var entityId = id.ToString();
        var stopwatch = Stopwatch.StartNew();

        BusinessLogMessages.UpdatingEntity(_logger, "MenuItem", entityId);

        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            BusinessLogMessages.EntityNotFound(_logger, "MenuItem", entityId);
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

            BusinessLogMessages.EntityUpdated(_logger, "MenuItem", entityId);
            BusinessLogMessages.ServiceOperationCompleted(_logger, "MenuItemService", "UpdateAsync", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            BusinessLogMessages.BusinessOperationFailed(_logger, ex, "UpdateAsync", "MenuItem", entityId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entityId = id.ToString();
        var stopwatch = Stopwatch.StartNew();

        BusinessLogMessages.DeletingEntity(_logger, "MenuItem", entityId);

        try
        {
            var result = await _repository.DeleteAsync(id, cancellationToken);
            stopwatch.Stop();

            if (result)
            {
                BusinessLogMessages.EntityDeleted(_logger, "MenuItem", entityId);
                BusinessLogMessages.ServiceOperationCompleted(_logger, "MenuItemService", "DeleteAsync", stopwatch.ElapsedMilliseconds);
            }
            else
            {
                BusinessLogMessages.EntityNotFound(_logger, "MenuItem", entityId);
            }

            return result;
        }
        catch (Exception ex)
        {
            BusinessLogMessages.BusinessOperationFailed(_logger, ex, "DeleteAsync", "MenuItem", entityId);
            throw;
        }
    }
}
