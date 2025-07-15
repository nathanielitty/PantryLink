namespace PantryLink.Core.Entities;

public class ZipDistance
{
    public int Id { get; set; }
    public string FromZipCode { get; set; } = string.Empty;
    public string ToZipCode { get; set; } = string.Empty;
    public double DistanceMiles { get; set; }
    public int EstimatedTravelTimeMinutes { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsVerified { get; set; } = true;
}
