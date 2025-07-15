using System.ComponentModel.DataAnnotations;
using PantryLink.Core.Entities;

namespace PantryLink.Core.DTOs;

public class PantryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public TimeSpan? OpenTime { get; set; }
    public TimeSpan? CloseTime { get; set; }
    public double AverageRating { get; set; }
    public int TotalRatings { get; set; }
    public int TotalInventoryItems { get; set; }
}

public class CreatePantryDto
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(5, MinimumLength = 5)]
    public string ZipCode { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? Description { get; set; }
    public TimeSpan? OpenTime { get; set; }
    public TimeSpan? CloseTime { get; set; }
}

public class InventoryItemDto
{
    public int Id { get; set; }
    public int PantryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime? ExpirationDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsVegan { get; set; }
    public bool IsGlutenFree { get; set; }
    public bool IsHalal { get; set; }
    public bool IsKosher { get; set; }
    public int Priority { get; set; }
}

public class CreateInventoryItemDto
{
    [Required]
    public int PantryId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? Barcode { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = "units";
    
    public DateTime? ExpirationDate { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
    
    public bool IsVegan { get; set; } = false;
    public bool IsGlutenFree { get; set; } = false;
    public bool IsHalal { get; set; } = false;
    public bool IsKosher { get; set; } = false;
}

public class UpdateStockDto
{
    [Required]
    [MaxLength(50)]
    public string Barcode { get; set; } = string.Empty;
    
    [Required]
    public int QuantityChange { get; set; } // Can be positive or negative
}

public class UserPreferencesDto
{
    [Required]
    public IEnumerable<DietaryPreference> Preferences { get; set; } = new List<DietaryPreference>();
}

public class PantryRecommendationDto
{
    public PantryDto Pantry { get; set; } = null!;
    public IEnumerable<InventoryItemDto> RecommendedItems { get; set; } = new List<InventoryItemDto>();
    public int Score { get; set; }
    public string ReasonForRecommendation { get; set; } = string.Empty;
}

// Analytics DTOs
public class PantryAnalyticsDto
{
    public int Id { get; set; }
    public int PantryId { get; set; }
    public string PantryName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalQuantity { get; set; }
    public int ExpiringItemsCount { get; set; }
    public int VisitorCount { get; set; }
    public int InventoryUpdatesCount { get; set; }
    public double AverageRating { get; set; }
    public string? MostPopularCategory { get; set; }
    public int ItemsDistributedCount { get; set; }
}

public class SystemAnalyticsDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int TotalActivePantries { get; set; }
    public int TotalUsers { get; set; }
    public int TotalInventoryItems { get; set; }
    public int TotalSearches { get; set; }
    public int TotalRecommendations { get; set; }
    public string? MostSearchedZipCode { get; set; }
    public int TotalItemsDistributed { get; set; }
    public double AverageSystemRating { get; set; }
}

public class AnalyticsDashboardDto
{
    public SystemAnalyticsDto SystemStats { get; set; } = null!;
    public IEnumerable<PantryAnalyticsDto> TopPerformingPantries { get; set; } = new List<PantryAnalyticsDto>();
    public Dictionary<string, int> PopularCategories { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> SearchesByZipCode { get; set; } = new Dictionary<string, int>();
    public IEnumerable<PantryAnalyticsDto> RecentActivity { get; set; } = new List<PantryAnalyticsDto>();
}

public class ZipDistanceDto
{
    public int Id { get; set; }
    public string FromZipCode { get; set; } = string.Empty;
    public string ToZipCode { get; set; } = string.Empty;
    public double DistanceMiles { get; set; }
    public int EstimatedTravelTimeMinutes { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsVerified { get; set; }
}
