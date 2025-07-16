using System.ComponentModel.DataAnnotations;

namespace PantryLink.Core.Entities;

public class Pantry
{
    public int Id { get; set; }
    
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
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Rating calculation
    public double AverageRating { get; set; } = 0.0;
    public int TotalRatings { get; set; } = 0;
    
    // Navigation properties
    public virtual ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
    public virtual ICollection<PantryAdmin> Admins { get; set; } = new List<PantryAdmin>();
    public virtual ICollection<PantryRating> Ratings { get; set; } = new List<PantryRating>();
}
