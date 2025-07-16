using System.ComponentModel.DataAnnotations;

namespace PantryLink.Core.Entities;

public enum DietaryPreference
{
    Vegan,
    Vegetarian,
    Halal,
    Kosher,
    GlutenFree,
    DairyFree,
    NutFree
}

public class UserPreference
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    public DietaryPreference Preference { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}
