using System.ComponentModel.DataAnnotations;

namespace PantryLink.Models
{
    public class FoodDonation
    {
        public int Id { get; set; }
        public int PantryId { get; set; }
        public virtual Pantry Pantry { get; set; }
        
        [Required]
        public string DonorName { get; set; }
        
        [Required]
        [EmailAddress]
        public string DonorEmail { get; set; }
        
        [Required]
        public string DonorPhone { get; set; }
        
        public string Notes { get; set; }
        public DateTime PreferredPickupTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Approved, Picked Up, Completed
        
        public virtual ICollection<DonationItem> Items { get; set; } = new List<DonationItem>();
    }
    
    public class DonationItem
    {
        public int Id { get; set; }
        public int FoodDonationId { get; set; }
        public virtual FoodDonation FoodDonation { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        public string Category { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    
    public class MonetaryDonation
    {
        public int Id { get; set; }
        public int PantryId { get; set; }
        public virtual Pantry Pantry { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string DonorName { get; set; }
        
        [Required]
        [EmailAddress]
        public string DonorEmail { get; set; }
        
        [Required]
        public string DonorPhone { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        
        public bool IsRecurring { get; set; } = false;
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Processed, Completed, Failed
        public string TransactionId { get; set; }
    }
}
