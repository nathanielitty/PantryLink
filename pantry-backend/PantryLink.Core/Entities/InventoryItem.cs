using System.ComponentModel.DataAnnotations;

namespace PantryLink.Core.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    
    public int PantryId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? Barcode { get; set; }
    
    public int Quantity { get; set; } = 0;
    
    [MaxLength(50)]
    public string Unit { get; set; } = "units"; // units, lbs, oz, cans, etc.
    
    public DateTime? ExpirationDate { get; set; }
    
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty; // Canned Goods, Fresh Produce, etc.
    
    // USDA FoodData Central ID for nutritional info
    public string? UsdaFoodId { get; set; }
    
    // Dietary flags for AI recommendations
    public bool IsVegan { get; set; } = false;
    public bool IsGlutenFree { get; set; } = false;
    public bool IsHalal { get; set; } = false;
    public bool IsKosher { get; set; } = false;
    
    // Priority calculation for recommendations
    public int Priority => CalculatePriority();
    
    // Navigation properties
    public virtual Pantry Pantry { get; set; } = null!;
    
    private int CalculatePriority()
    {
        var priority = 0;
        
        // Higher priority for items expiring soon
        if (ExpirationDate.HasValue)
        {
            var daysUntilExpiry = (ExpirationDate.Value - DateTime.UtcNow).Days;
            if (daysUntilExpiry <= 3) priority += 100;
            else if (daysUntilExpiry <= 7) priority += 50;
            else if (daysUntilExpiry <= 14) priority += 25;
        }
        
        // Higher priority for high stock items
        if (Quantity > 50) priority += 20;
        else if (Quantity > 20) priority += 10;
        
        return priority;
    }
}
