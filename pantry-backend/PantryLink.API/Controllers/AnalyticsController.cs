using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryLink.Core.DTOs;
using PantryLink.Core.Interfaces;

namespace PantryLink.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IZipDistanceService _zipDistanceService;
    private readonly IMapper _mapper;

    public AnalyticsController(
        IAnalyticsService analyticsService, 
        IZipDistanceService zipDistanceService,
        IMapper mapper)
    {
        _analyticsService = analyticsService;
        _zipDistanceService = zipDistanceService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get comprehensive dashboard analytics
    /// </summary>
    [HttpGet("dashboard")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AnalyticsDashboardDto>> GetDashboard()
    {
        var dashboard = await _analyticsService.GetDashboardDataAsync();
        return Ok(dashboard);
    }

    /// <summary>
    /// Get system-wide analytics for a specific date
    /// </summary>
    [HttpGet("system")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SystemAnalyticsDto>> GetSystemAnalytics([FromQuery] DateTime? date = null)
    {
        var analytics = await _analyticsService.GetSystemAnalyticsAsync(date);
        var dto = new SystemAnalyticsDto
        {
            Id = analytics.Id,
            Date = analytics.Date,
            TotalActivePantries = analytics.TotalActivePantries,
            TotalUsers = analytics.TotalUsers,
            TotalInventoryItems = analytics.TotalInventoryItems,
            TotalSearches = analytics.TotalSearches,
            TotalRecommendations = analytics.TotalRecommendations,
            MostSearchedZipCode = analytics.MostSearchedZipCode,
            TotalItemsDistributed = analytics.TotalItemsDistributed,
            AverageSystemRating = analytics.AverageSystemRating
        };
        
        return Ok(dto);
    }

    /// <summary>
    /// Get analytics for a specific pantry
    /// </summary>
    [HttpGet("pantry/{pantryId}")]
    [Authorize(Roles = "Admin,PantryAdmin")]
    public async Task<ActionResult<PantryAnalyticsDto>> GetPantryAnalytics(int pantryId, [FromQuery] DateTime? date = null)
    {
        var analytics = await _analyticsService.GetPantryAnalyticsAsync(pantryId, date);
        var dto = new PantryAnalyticsDto
        {
            Id = analytics.Id,
            PantryId = analytics.PantryId,
            PantryName = analytics.Pantry?.Name ?? "",
            Date = analytics.Date,
            TotalItemsCount = analytics.TotalItemsCount,
            TotalQuantity = analytics.TotalQuantity,
            ExpiringItemsCount = analytics.ExpiringItemsCount,
            VisitorCount = analytics.VisitorCount,
            InventoryUpdatesCount = analytics.InventoryUpdatesCount,
            AverageRating = analytics.AverageRating,
            MostPopularCategory = analytics.MostPopularCategory,
            ItemsDistributedCount = analytics.ItemsDistributedCount
        };
        
        return Ok(dto);
    }

    /// <summary>
    /// Get top performing pantries
    /// </summary>
    [HttpGet("top-pantries")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<PantryAnalyticsDto>>> GetTopPerformingPantries([FromQuery] int count = 10)
    {
        var pantries = await _analyticsService.GetTopPerformingPantriesAsync(count);
        var dtos = pantries.Select(pa => new PantryAnalyticsDto
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
        });
        
        return Ok(dtos);
    }

    /// <summary>
    /// Get popular food categories across all pantries
    /// </summary>
    [HttpGet("popular-categories")]
    public async Task<ActionResult<Dictionary<string, int>>> GetPopularCategories()
    {
        var categories = await _analyticsService.GetPopularCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Get search statistics by ZIP code
    /// </summary>
    [HttpGet("searches-by-zip")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Dictionary<string, int>>> GetSearchesByZipCode()
    {
        var searches = await _analyticsService.GetSearchesByZipCodeAsync();
        return Ok(searches);
    }

    /// <summary>
    /// Update analytics for a specific pantry (manually trigger)
    /// </summary>
    [HttpPost("update-pantry/{pantryId}")]
    [Authorize(Roles = "Admin,PantryAdmin")]
    public async Task<IActionResult> UpdatePantryAnalytics(int pantryId)
    {
        await _analyticsService.UpdatePantryAnalyticsAsync(pantryId);
        return Ok("Analytics updated successfully");
    }

    /// <summary>
    /// Update system-wide analytics (manually trigger)
    /// </summary>
    [HttpPost("update-system")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSystemAnalytics()
    {
        await _analyticsService.UpdateSystemAnalyticsAsync();
        return Ok("System analytics updated successfully");
    }

    /// <summary>
    /// Get distance between two ZIP codes
    /// </summary>
    [HttpGet("distance")]
    public async Task<ActionResult<ZipDistanceDto>> GetDistance([FromQuery] string fromZip, [FromQuery] string toZip)
    {
        if (string.IsNullOrWhiteSpace(fromZip) || string.IsNullOrWhiteSpace(toZip))
        {
            return BadRequest("Both fromZip and toZip are required");
        }

        var distance = await _zipDistanceService.GetDistanceAsync(fromZip, toZip);
        if (distance == null)
        {
            // Calculate and cache the distance
            var calculatedDistance = await _zipDistanceService.CalculateDistanceAsync(fromZip, toZip);
            distance = await _zipDistanceService.GetDistanceAsync(fromZip, toZip);
        }

        if (distance == null)
        {
            return NotFound("Distance calculation failed");
        }

        var dto = new ZipDistanceDto
        {
            Id = distance.Id,
            FromZipCode = distance.FromZipCode,
            ToZipCode = distance.ToZipCode,
            DistanceMiles = distance.DistanceMiles,
            EstimatedTravelTimeMinutes = distance.EstimatedTravelTimeMinutes,
            LastUpdated = distance.LastUpdated,
            IsVerified = distance.IsVerified
        };

        return Ok(dto);
    }

    /// <summary>
    /// Get nearby ZIP codes within a certain distance
    /// </summary>
    [HttpGet("nearby-zips")]
    public async Task<ActionResult<IEnumerable<ZipDistanceDto>>> GetNearbyZipCodes(
        [FromQuery] string zipCode, 
        [FromQuery] double maxDistance = 25.0)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
        {
            return BadRequest("ZIP code is required");
        }

        var nearbyDistances = await _zipDistanceService.GetNearbyZipCodesAsync(zipCode, maxDistance);
        var dtos = nearbyDistances.Select(zd => new ZipDistanceDto
        {
            Id = zd.Id,
            FromZipCode = zd.FromZipCode,
            ToZipCode = zd.ToZipCode,
            DistanceMiles = zd.DistanceMiles,
            EstimatedTravelTimeMinutes = zd.EstimatedTravelTimeMinutes,
            LastUpdated = zd.LastUpdated,
            IsVerified = zd.IsVerified
        });

        return Ok(dtos);
    }
}
