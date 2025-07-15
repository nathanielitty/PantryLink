using Microsoft.EntityFrameworkCore;
using PantryLink.Core.Algorithms;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;
using PantryLink.Infrastructure.Data;

namespace PantryLink.Infrastructure.Services;

public class PantryService : IPantryService
{
    private readonly ApplicationDbContext _context;
    private readonly IZipDistanceService _zipDistanceService;

    public PantryService(ApplicationDbContext context, IZipDistanceService zipDistanceService)
    {
        _context = context;
        _zipDistanceService = zipDistanceService;
    }

    public async Task<IEnumerable<Pantry>> GetPantriesByZipCodeAsync(string zipCode)
    {
        // Get pantries in the exact ZIP code first
        var exactMatches = await _context.Pantries
            .Where(p => p.ZipCode == zipCode && p.IsActive)
            .Include(p => p.Inventory)
            .ToListAsync();

        // Get nearby pantries within 25 miles
        var nearbyZipCodes = await _zipDistanceService.GetNearbyZipCodesAsync(zipCode, 25.0);
        var nearbyZips = nearbyZipCodes.Select(zd => 
            zd.FromZipCode == zipCode ? zd.ToZipCode : zd.FromZipCode).ToList();

        var nearbyPantries = await _context.Pantries
            .Where(p => nearbyZips.Contains(p.ZipCode) && p.IsActive)
            .Include(p => p.Inventory)
            .ToListAsync();

        // Combine and sort by distance, then rating
        var allPantries = exactMatches.Concat(nearbyPantries).Distinct();
        
        return allPantries.OrderBy(p => p.ZipCode == zipCode ? 0 : 1) // Exact matches first
                         .ThenByDescending(p => p.AverageRating)
                         .ToList();
    }

    public async Task<Pantry?> GetPantryByIdAsync(int id)
    {
        return await _context.Pantries
            .Include(p => p.Inventory)
            .Include(p => p.Admins)
            .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pantry> CreatePantryAsync(Pantry pantry)
    {
        _context.Pantries.Add(pantry);
        await _context.SaveChangesAsync();
        return pantry;
    }

    public async Task<Pantry> UpdatePantryAsync(Pantry pantry)
    {
        pantry.UpdatedAt = DateTime.UtcNow;
        _context.Pantries.Update(pantry);
        await _context.SaveChangesAsync();
        return pantry;
    }

    public async Task DeletePantryAsync(int id)
    {
        var pantry = await _context.Pantries.FindAsync(id);
        if (pantry != null)
        {
            pantry.IsActive = false;
            pantry.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Pantry>> GetRecommendedPantriesAsync(int userId, string zipCode)
    {
        // Get user preferences
        var userPreferences = await _context.UserPreferences
            .Where(up => up.UserId == userId)
            .Select(up => up.Preference)
            .ToListAsync();

        // Get pantries in the ZIP code
        var pantries = await _context.Pantries
            .Where(p => p.ZipCode == zipCode && p.IsActive)
            .Include(p => p.Inventory)
            .ToListAsync();

        // Use priority queue to rank pantries
        var priorityQueue = new PantryPriorityQueue();

        foreach (var pantry in pantries)
        {
            // Filter inventory based on user preferences
            if (userPreferences.Any())
            {
                var matchingItems = pantry.Inventory.Where(item =>
                    userPreferences.Any(pref => pref switch
                    {
                        DietaryPreference.Vegan => item.IsVegan,
                        DietaryPreference.GlutenFree => item.IsGlutenFree,
                        DietaryPreference.Halal => item.IsHalal,
                        DietaryPreference.Kosher => item.IsKosher,
                        _ => true
                    })
                ).ToList();

                // Create a copy of pantry with filtered inventory for ranking
                var filteredPantry = new Pantry
                {
                    Id = pantry.Id,
                    Name = pantry.Name,
                    ZipCode = pantry.ZipCode,
                    Address = pantry.Address,
                    PhoneNumber = pantry.PhoneNumber,
                    Email = pantry.Email,
                    Description = pantry.Description,
                    OpenTime = pantry.OpenTime,
                    CloseTime = pantry.CloseTime,
                    AverageRating = pantry.AverageRating,
                    TotalRatings = pantry.TotalRatings,
                    Inventory = matchingItems
                };

                if (matchingItems.Any())
                {
                    priorityQueue.Enqueue(filteredPantry);
                }
            }
            else
            {
                priorityQueue.Enqueue(pantry);
            }
        }

        return priorityQueue.GetSortedPantries();
    }
}
