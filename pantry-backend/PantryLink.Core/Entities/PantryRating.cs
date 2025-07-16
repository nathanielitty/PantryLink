using System.ComponentModel.DataAnnotations;

namespace PantryLink.Core.Entities;

public class PantryRating
{
    public int Id { get; set; }
    
    public int PantryId { get; set; }
    public int UserId { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [MaxLength(1000)]
    public string? Comment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Pantry Pantry { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
