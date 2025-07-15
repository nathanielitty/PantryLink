namespace PantryLink.Core.Entities;

public class PantryAnalytics
{
    public int Id { get; set; }
    public int PantryId { get; set; }
    public Pantry Pantry { get; set; } = null!;
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

public class SystemAnalytics
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
