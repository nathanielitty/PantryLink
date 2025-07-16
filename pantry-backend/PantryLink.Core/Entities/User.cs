using Microsoft.AspNetCore.Identity;

namespace PantryLink.Core.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    public virtual ICollection<UserPreference> Preferences { get; set; } = new List<UserPreference>();
    public virtual ICollection<PantryAdmin> PantryAdmins { get; set; } = new List<PantryAdmin>();
}
