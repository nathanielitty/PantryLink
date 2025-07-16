using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PantryLink.Core.Entities;

namespace PantryLink.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Pantry> Pantries { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<PantryAdmin> PantryAdmins { get; set; }
    public DbSet<PantryRating> PantryRatings { get; set; }
    public DbSet<ZipDistance> ZipDistances { get; set; }
    public DbSet<PantryAnalytics> PantryAnalytics { get; set; }
    public DbSet<SystemAnalytics> SystemAnalytics { get; set; }
    public DbSet<FoodDonation> FoodDonations { get; set; }
    public DbSet<DonationItem> DonationItems { get; set; }
    public DbSet<MonetaryDonation> MonetaryDonations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Pantry configuration
        builder.Entity<Pantry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ZipCode).HasDatabaseName("idx_zip");
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.ZipCode).HasMaxLength(5).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.AverageRating).HasPrecision(3, 2);
        });

        // InventoryItem configuration
        builder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PantryId);
            entity.HasIndex(e => e.Barcode);
            entity.HasIndex(e => e.Category);
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.UsdaFoodId).HasMaxLength(50);

            entity.HasOne(e => e.Pantry)
                  .WithMany(p => p.Inventory)
                  .HasForeignKey(e => e.PantryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // UserPreference configuration
        builder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Preference }).IsUnique();
            entity.Property(e => e.Preference).HasConversion<string>();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Preferences)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PantryAdmin configuration
        builder.Entity<PantryAdmin>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.PantryId }).IsUnique();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.PantryAdmins)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Pantry)
                  .WithMany(p => p.Admins)
                  .HasForeignKey(e => e.PantryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PantryRating configuration
        builder.Entity<PantryRating>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.PantryId }).IsUnique();

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Pantry)
                  .WithMany(p => p.Ratings)
                  .HasForeignKey(e => e.PantryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        // Seed some sample pantries
        builder.Entity<Pantry>().HasData(
            new Pantry
            {
                Id = 1,
                Name = "Community Food Bank",
                ZipCode = "12345",
                Address = "123 Main St, Anytown, ST",
                PhoneNumber = "(555) 123-4567",
                Email = "info@communityfoodbank.org",
                Description = "Serving the community since 1985",
                OpenTime = new TimeSpan(9, 0, 0),
                CloseTime = new TimeSpan(17, 0, 0),
                AverageRating = 4.5,
                TotalRatings = 42
            },
            new Pantry
            {
                Id = 2,
                Name = "Hope Center Pantry",
                ZipCode = "12345",
                Address = "456 Oak Ave, Anytown, ST",
                PhoneNumber = "(555) 987-6543",
                Email = "contact@hopecenter.org",
                Description = "Emergency food assistance",
                OpenTime = new TimeSpan(10, 0, 0),
                CloseTime = new TimeSpan(16, 0, 0),
                AverageRating = 4.2,
                TotalRatings = 28
            }
        );

        // ZipDistance configuration
        builder.Entity<ZipDistance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.FromZipCode, e.ToZipCode }).IsUnique().HasDatabaseName("idx_zip_pair");
            entity.Property(e => e.FromZipCode).HasMaxLength(5).IsRequired();
            entity.Property(e => e.ToZipCode).HasMaxLength(5).IsRequired();
            entity.Property(e => e.DistanceMiles).HasPrecision(8, 2);
        });

        // PantryAnalytics configuration
        builder.Entity<PantryAnalytics>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.PantryId, e.Date }).IsUnique().HasDatabaseName("idx_pantry_date");
            entity.HasOne(e => e.Pantry)
                  .WithMany()
                  .HasForeignKey(e => e.PantryId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.MostPopularCategory).HasMaxLength(100);
            entity.Property(e => e.AverageRating).HasPrecision(3, 2);
        });

        // SystemAnalytics configuration
        builder.Entity<SystemAnalytics>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Date).IsUnique().HasDatabaseName("idx_system_date");
            entity.Property(e => e.MostSearchedZipCode).HasMaxLength(5);
            entity.Property(e => e.AverageSystemRating).HasPrecision(3, 2);
        });

        // Seed ZipDistance data
        builder.Entity<ZipDistance>().HasData(
            new ZipDistance
            {
                Id = 1,
                FromZipCode = "12345",
                ToZipCode = "12346",
                DistanceMiles = 2.5,
                EstimatedTravelTimeMinutes = 8,
                LastUpdated = DateTime.UtcNow,
                IsVerified = true
            },
            new ZipDistance
            {
                Id = 2,
                FromZipCode = "12345",
                ToZipCode = "12347",
                DistanceMiles = 5.1,
                EstimatedTravelTimeMinutes = 15,
                LastUpdated = DateTime.UtcNow,
                IsVerified = true
            },
            new ZipDistance
            {
                Id = 3,
                FromZipCode = "12346",
                ToZipCode = "12345",
                DistanceMiles = 2.5,
                EstimatedTravelTimeMinutes = 8,
                LastUpdated = DateTime.UtcNow,
                IsVerified = true
            }
        );

        // FoodDonation configuration
        builder.Entity<FoodDonation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DonorName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.DonorEmail).HasMaxLength(255).IsRequired();
            entity.Property(e => e.DonorPhone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();

            entity.HasOne(e => e.Pantry)
                  .WithMany()
                  .HasForeignKey(e => e.PantryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // DonationItem configuration
        builder.Entity<DonationItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(100);

            entity.HasOne(e => e.FoodDonation)
                  .WithMany(fd => fd.Items)
                  .HasForeignKey(e => e.FoodDonationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // MonetaryDonation configuration
        builder.Entity<MonetaryDonation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.DonorName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.DonorEmail).HasMaxLength(255).IsRequired();
            entity.Property(e => e.DonorPhone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.PaymentMethod).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TransactionId).HasMaxLength(100);

            entity.HasOne(e => e.Pantry)
                  .WithMany()
                  .HasForeignKey(e => e.PantryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
