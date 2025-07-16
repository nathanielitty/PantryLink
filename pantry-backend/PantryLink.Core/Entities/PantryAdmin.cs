namespace PantryLink.Core.Entities;

public class PantryAdmin
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public int PantryId { get; set; }
    
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Pantry Pantry { get; set; } = null!;
}
