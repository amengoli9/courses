using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Domain.Entities;
using RestaurantMenu.Domain.Enums;
using RestaurantMenu.Domain.Repositories;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly RestaurantDbContext _context;

    public MenuItemRepository(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .Where(x => x.Category == category)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .Where(x => x.IsAvailable)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<MenuItem> AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        await _context.MenuItems.AddAsync(menuItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return menuItem;
    }

    public async Task<MenuItem> UpdateAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync(cancellationToken);
        return menuItem;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menuItem = await _context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
        if (menuItem is null)
            return false;

        _context.MenuItems.Remove(menuItem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems.AnyAsync(x => x.Id == id, cancellationToken);
    }
}
