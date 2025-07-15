using Microsoft.EntityFrameworkCore;
using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;
using PantryLink.Infrastructure.Data;

namespace PantryLink.Infrastructure.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SystemAnalytics> GetSystemAnalyticsAsync(DateTime? date = null)
    {
        var targetDate = date ?? DateTime.Today;
        
        var existing = await _context.SystemAnalytics
            .FirstOrDefaultAsync(sa => sa.Date.Date == targetDate.Date);

        if (existing != null)
        {
            return existing;
        }

        // Generate fresh analytics
        await UpdateSystemAnalyticsAsync();
        return await _context.SystemAnalytics
            .FirstOrDefaultAsync(sa => sa.Date.Date == targetDate.Date) ?? new SystemAnalytics();
    }

    public async Task<PantryAnalytics> GetPantryAnalyticsAsync(int pantryId, DateTime? date = null)
    {
        var targetDate = date ?? DateTime.Today;
        
        var existing = await _context.PantryAnalytics
            .Include(pa => pa.Pantry)
            .FirstOrDefaultAsync(pa => pa.PantryId == pantryId && pa.Date.Date == targetDate.Date);

        if (existing != null)
        {
            return existing;
        }

        // Generate fresh analytics
        await UpdatePantryAnalyticsAsync(pantryId);
        return await _context.PantryAnalytics
            .Include(pa => pa.Pantry)
            .FirstOrDefaultAsync(pa => pa.PantryId == pantryId && pa.Date.Date == targetDate.Date) ?? new PantryAnalytics();
    }

    public async Task<IEnumerable<PantryAnalytics>> GetTopPerformingPantriesAsync(int count = 10)
    {
        var today = DateTime.Today;
        return await _context.PantryAnalytics
            .Include(pa => pa.Pantry)
            .Where(pa => pa.Date.Date == today.Date)
            .OrderByDescending(pa => pa.AverageRating)
            .ThenByDescending(pa => pa.VisitorCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<AnalyticsDashboardDto> GetDashboardDataAsync()
    {
        var systemStats = await GetSystemAnalyticsAsync();
        var topPantries = await GetTopPerformingPantriesAsync(5);
        var popularCategories = await GetPopularCategoriesAsync();
        var searchesByZip = await GetSearchesByZipCodeAsync();
        
        var recentActivity = await _context.PantryAnalytics
            .Include(pa => pa.Pantry)
            .Where(pa => pa.Date >= DateTime.Today.AddDays(-7))
            .OrderByDescending(pa => pa.Date)
            .Take(10)
            .ToListAsync();

        return new AnalyticsDashboardDto
        {
            SystemStats = new SystemAnalyticsDto
            {
                Id = systemStats.Id,
                Date = systemStats.Date,
                TotalActivePantries = systemStats.TotalActivePantries,
                TotalUsers = systemStats.TotalUsers,
                TotalInventoryItems = systemStats.TotalInventoryItems,
                TotalSearches = systemStats.TotalSearches,
                TotalRecommendations = systemStats.TotalRecommendations,
                MostSearchedZipCode = systemStats.MostSearchedZipCode,
                TotalItemsDistributed = systemStats.TotalItemsDistributed,
                AverageSystemRating = systemStats.AverageSystemRating
            },
            TopPerformingPantries = topPantries.Select(pa => new PantryAnalyticsDto
            {
                Id = pa.Id,
                PantryId = pa.PantryId,
                PantryName = pa.Pantry.Name,
                Date = pa.Date,
                TotalItemsCount = pa.TotalItemsCount,
                TotalQuantity = pa.TotalQuantity,
                ExpiringItemsCount = pa.ExpiringItemsCount,
                VisitorCount = pa.VisitorCount,
                InventoryUpdatesCount = pa.InventoryUpdatesCount,
                AverageRating = pa.AverageRating,
                MostPopularCategory = pa.MostPopularCategory,
                ItemsDistributedCount = pa.ItemsDistributedCount
            }),
            PopularCategories = popularCategories,
            SearchesByZipCode = searchesByZip,
            RecentActivity = recentActivity.Select(pa => new PantryAnalyticsDto
            {
                Id = pa.Id,
                PantryId = pa.PantryId,
                PantryName = pa.Pantry.Name,
                Date = pa.Date,
                TotalItemsCount = pa.TotalItemsCount,
                TotalQuantity = pa.TotalQuantity,
                ExpiringItemsCount = pa.ExpiringItemsCount,
                VisitorCount = pa.VisitorCount,
                InventoryUpdatesCount = pa.InventoryUpdatesCount,
                AverageRating = pa.AverageRating,
                MostPopularCategory = pa.MostPopularCategory,
                ItemsDistributedCount = pa.ItemsDistributedCount
            })
        };
    }

    public async Task UpdatePantryAnalyticsAsync(int pantryId)
    {
        var today = DateTime.Today;
        var pantry = await _context.Pantries
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.Id == pantryId);

        if (pantry == null) return;

        var existing = await _context.PantryAnalytics
            .FirstOrDefaultAsync(pa => pa.PantryId == pantryId && pa.Date.Date == today.Date);

        var totalItems = pantry.Inventory.Count;
        var totalQuantity = pantry.Inventory.Sum(i => i.Quantity);
        var expiringItems = pantry.Inventory.Count(i => 
            i.ExpirationDate.HasValue && 
            (i.ExpirationDate.Value - DateTime.Now).Days <= 7);
        
        var mostPopularCategory = pantry.Inventory
            .GroupBy(i => i.Category)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key;

        if (existing != null)
        {
            existing.TotalItemsCount = totalItems;
            existing.TotalQuantity = totalQuantity;
            existing.ExpiringItemsCount = expiringItems;
            existing.AverageRating = pantry.AverageRating;
            existing.MostPopularCategory = mostPopularCategory;
            existing.VisitorCount += 1; // Increment on update
            existing.InventoryUpdatesCount += 1;
        }
        else
        {
            var newAnalytics = new PantryAnalytics
            {
                PantryId = pantryId,
                Date = today,
                TotalItemsCount = totalItems,
                TotalQuantity = totalQuantity,
                ExpiringItemsCount = expiringItems,
                VisitorCount = 1,
                InventoryUpdatesCount = 1,
                AverageRating = pantry.AverageRating,
                MostPopularCategory = mostPopularCategory,
                ItemsDistributedCount = 0
            };

            _context.PantryAnalytics.Add(newAnalytics);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateSystemAnalyticsAsync()
    {
        var today = DateTime.Today;
        var existing = await _context.SystemAnalytics
            .FirstOrDefaultAsync(sa => sa.Date.Date == today.Date);

        var totalPantries = await _context.Pantries.CountAsync(p => p.IsActive);
        var totalUsers = await _context.Users.CountAsync();
        var totalInventoryItems = await _context.InventoryItems.CountAsync();
        var avgRating = await _context.Pantries.AverageAsync(p => p.AverageRating);

        // Simulate search and recommendation counts (in a real app, these would be tracked)
        var totalSearches = await _context.PantryAnalytics.SumAsync(pa => pa.VisitorCount);
        var totalRecommendations = totalSearches * 3; // Estimate 3 recommendations per search

        // Find most searched ZIP (simulated from pantry analytics)
        var mostSearchedZip = await _context.PantryAnalytics
            .Include(pa => pa.Pantry)
            .GroupBy(pa => pa.Pantry.ZipCode)
            .OrderByDescending(g => g.Sum(pa => pa.VisitorCount))
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        var totalDistributed = await _context.PantryAnalytics.SumAsync(pa => pa.ItemsDistributedCount);

        if (existing != null)
        {
            existing.TotalActivePantries = totalPantries;
            existing.TotalUsers = totalUsers;
            existing.TotalInventoryItems = totalInventoryItems;
            existing.TotalSearches = totalSearches;
            existing.TotalRecommendations = totalRecommendations;
            existing.MostSearchedZipCode = mostSearchedZip;
            existing.TotalItemsDistributed = totalDistributed;
            existing.AverageSystemRating = avgRating;
        }
        else
        {
            var newSystemAnalytics = new SystemAnalytics
            {
                Date = today,
                TotalActivePantries = totalPantries,
                TotalUsers = totalUsers,
                TotalInventoryItems = totalInventoryItems,
                TotalSearches = totalSearches,
                TotalRecommendations = totalRecommendations,
                MostSearchedZipCode = mostSearchedZip,
                TotalItemsDistributed = totalDistributed,
                AverageSystemRating = avgRating
            };

            _context.SystemAnalytics.Add(newSystemAnalytics);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<string, int>> GetPopularCategoriesAsync()
    {
        return await _context.InventoryItems
            .GroupBy(i => i.Category)
            .Select(g => new { Category = g.Key, Count = g.Sum(i => i.Quantity) })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToDictionaryAsync(x => x.Category, x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetSearchesByZipCodeAsync()
    {
        return await _context.PantryAnalytics
            .Include(pa => pa.Pantry)
            .GroupBy(pa => pa.Pantry.ZipCode)
            .Select(g => new { ZipCode = g.Key, Searches = g.Sum(pa => pa.VisitorCount) })
            .OrderByDescending(x => x.Searches)
            .Take(10)
            .ToDictionaryAsync(x => x.ZipCode, x => x.Searches);
    }
}
