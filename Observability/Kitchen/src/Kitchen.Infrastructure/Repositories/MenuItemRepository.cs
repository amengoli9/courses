using Microsoft.EntityFrameworkCore;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;
using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;

namespace Kitchen.Infrastructure.Repositories;

public class MenuItemRepository(RestaurantDbContext context) : IMenuItemRepository
{

    public async Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.MenuItems
            .AsNoTracking()
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default)
    {
        return await context.MenuItems
            .AsNoTracking()
            .Where(x => x.Category == category)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await context.MenuItems
            .AsNoTracking()
            .Where(x => x.IsAvailable)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<MenuItem> AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        await context.MenuItems.AddAsync(menuItem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return menuItem;
    }

    public async Task<MenuItem> UpdateAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        context.MenuItems.Update(menuItem);
        await context.SaveChangesAsync(cancellationToken);
        return menuItem;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menuItem = await context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
        if (menuItem is null)
            return false;

        context.MenuItems.Remove(menuItem);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.MenuItems.AnyAsync(x => x.Id == id, cancellationToken);
    }
}
