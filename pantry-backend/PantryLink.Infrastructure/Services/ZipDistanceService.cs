using Microsoft.EntityFrameworkCore;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;
using PantryLink.Infrastructure.Data;

namespace PantryLink.Infrastructure.Services;

public class ZipDistanceService : IZipDistanceService
{
    private readonly ApplicationDbContext _context;

    public ZipDistanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ZipDistance?> GetDistanceAsync(string fromZip, string toZip)
    {
        return await _context.ZipDistances
            .FirstOrDefaultAsync(zd => 
                (zd.FromZipCode == fromZip && zd.ToZipCode == toZip) ||
                (zd.FromZipCode == toZip && zd.ToZipCode == fromZip));
    }

    public async Task<IEnumerable<ZipDistance>> GetNearbyZipCodesAsync(string zipCode, double maxDistance)
    {
        return await _context.ZipDistances
            .Where(zd => 
                (zd.FromZipCode == zipCode || zd.ToZipCode == zipCode) &&
                zd.DistanceMiles <= maxDistance)
            .OrderBy(zd => zd.DistanceMiles)
            .ToListAsync();
    }

    public async Task<ZipDistance> AddOrUpdateDistanceAsync(string fromZip, string toZip, double distanceMiles, int travelTimeMinutes)
    {
        var existing = await GetDistanceAsync(fromZip, toZip);
        
        if (existing != null)
        {
            existing.DistanceMiles = distanceMiles;
            existing.EstimatedTravelTimeMinutes = travelTimeMinutes;
            existing.LastUpdated = DateTime.UtcNow;
            existing.IsVerified = true;
            
            await _context.SaveChangesAsync();
            return existing;
        }

        var newDistance = new ZipDistance
        {
            FromZipCode = fromZip,
            ToZipCode = toZip,
            DistanceMiles = distanceMiles,
            EstimatedTravelTimeMinutes = travelTimeMinutes,
            LastUpdated = DateTime.UtcNow,
            IsVerified = true
        };

        _context.ZipDistances.Add(newDistance);
        await _context.SaveChangesAsync();
        return newDistance;
    }

    public async Task<IEnumerable<ZipDistance>> GetAllDistancesForZipAsync(string zipCode)
    {
        return await _context.ZipDistances
            .Where(zd => zd.FromZipCode == zipCode || zd.ToZipCode == zipCode)
            .OrderBy(zd => zd.DistanceMiles)
            .ToListAsync();
    }

    public async Task<double> CalculateDistanceAsync(string fromZip, string toZip)
    {
        // First check if we have the distance cached
        var existing = await GetDistanceAsync(fromZip, toZip);
        if (existing != null)
        {
            return existing.DistanceMiles;
        }

        // In a real implementation, this would call a geocoding API like Google Maps
        // For demo purposes, we'll use a simple heuristic based on ZIP code differences
        var estimatedDistance = CalculateSimpleDistance(fromZip, toZip);
        var estimatedTime = (int)(estimatedDistance * 3); // Roughly 3 minutes per mile

        // Cache the result
        await AddOrUpdateDistanceAsync(fromZip, toZip, estimatedDistance, estimatedTime);
        
        return estimatedDistance;
    }

    private static double CalculateSimpleDistance(string fromZip, string toZip)
    {
        // Simple heuristic: calculate distance based on ZIP code numerical difference
        // In reality, you'd use geocoding APIs and haversine formula
        if (!int.TryParse(fromZip, out var from) || !int.TryParse(toZip, out var to))
        {
            return 10.0; // Default distance if ZIP codes aren't numeric
        }

        var difference = Math.Abs(from - to);
        
        // Very rough approximation: smaller differences = closer ZIP codes
        return difference switch
        {
            0 => 0.0,
            1 => 2.5,
            2 => 5.1,
            <= 5 => difference * 2.5,
            <= 10 => difference * 1.8,
            <= 50 => difference * 1.2,
            <= 100 => difference * 0.8,
            _ => difference * 0.5
        };
    }
}
