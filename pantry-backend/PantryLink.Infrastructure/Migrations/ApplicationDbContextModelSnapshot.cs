﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PantryLink.Infrastructure.Data;

#nullable disable

namespace PantryLink.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PantryLink.Core.Entities.DonationItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FoodDonationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("FoodDonationId");

                    b.ToTable("DonationItems");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.FoodDonation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DonorEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DonorName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DonorPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("PantryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PreferredPickupTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("PantryId");

                    b.ToTable("FoodDonations");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Barcode")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsGlutenFree")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsHalal")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsKosher")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsVegan")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("PantryId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UsdaFoodId")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Barcode");

                    b.HasIndex("Category");

                    b.HasIndex("PantryId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.MonetaryDonation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DonorEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DonorName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DonorPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("bit(1)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("PantryId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("PantryId");

                    b.ToTable("MonetaryDonations");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.Pantry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<double>("AverageRating")
                        .HasPrecision(3, 2)
                        .HasColumnType("double");

                    b.Property<TimeSpan?>("CloseTime")
                        .HasColumnType("time(6)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<TimeSpan?>("OpenTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("TotalRatings")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("ZipCode")
                        .HasDatabaseName("idx_zip");

                    b.ToTable("Pantries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St, Anytown, ST",
                            AverageRating = 4.5,
                            CloseTime = new TimeSpan(0, 17, 0, 0, 0),
                            CreatedAt = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4304),
                            Description = "Serving the community since 1985",
                            Email = "info@communityfoodbank.org",
                            IsActive = true,
                            Name = "Community Food Bank",
                            OpenTime = new TimeSpan(0, 9, 0, 0, 0),
                            PhoneNumber = "(555) 123-4567",
                            TotalRatings = 42,
                            UpdatedAt = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4304),
                            ZipCode = "12345"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Oak Ave, Anytown, ST",
                            AverageRating = 4.2000000000000002,
                            CloseTime = new TimeSpan(0, 16, 0, 0, 0),
                            CreatedAt = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4320),
                            Description = "Emergency food assistance",
                            Email = "contact@hopecenter.org",
                            IsActive = true,
                            Name = "Hope Center Pantry",
                            OpenTime = new TimeSpan(0, 10, 0, 0, 0),
                            PhoneNumber = "(555) 987-6543",
                            TotalRatings = 28,
                            UpdatedAt = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(4320),
                            ZipCode = "12345"
                        });
                });

            modelBuilder.Entity("PantryLink.Core.Entities.PantryAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit(1)");

                    b.Property<int>("PantryId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PantryId");

                    b.HasIndex("UserId", "PantryId")
                        .IsUnique();

                    b.ToTable("PantryAdmins");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.PantryAnalytics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("AverageRating")
                        .HasPrecision(3, 2)
                        .HasColumnType("double");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ExpiringItemsCount")
                        .HasColumnType("int");

                    b.Property<int>("InventoryUpdatesCount")
                        .HasColumnType("int");

                    b.Property<int>("ItemsDistributedCount")
                        .HasColumnType("int");

                    b.Property<string>("MostPopularCategory")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("PantryId")
                        .HasColumnType("int");

                    b.Property<int>("TotalItemsCount")
                        .HasColumnType("int");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.Property<int>("VisitorCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PantryId", "Date")
                        .IsUnique()
                        .HasDatabaseName("idx_pantry_date");

                    b.ToTable("PantryAnalytics");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.PantryRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PantryId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PantryId");

                    b.HasIndex("UserId", "PantryId")
                        .IsUnique();

                    b.ToTable("PantryRatings");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.SystemAnalytics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("AverageSystemRating")
                        .HasPrecision(3, 2)
                        .HasColumnType("double");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MostSearchedZipCode")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<int>("TotalActivePantries")
                        .HasColumnType("int");

                    b.Property<int>("TotalInventoryItems")
                        .HasColumnType("int");

                    b.Property<int>("TotalItemsDistributed")
                        .HasColumnType("int");

                    b.Property<int>("TotalRecommendations")
                        .HasColumnType("int");

                    b.Property<int>("TotalSearches")
                        .HasColumnType("int");

                    b.Property<int>("TotalUsers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Date")
                        .IsUnique()
                        .HasDatabaseName("idx_system_date");

                    b.ToTable("SystemAnalytics");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit(1)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("PantryLink.Core.Entities.UserPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Preference")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Preference")
                        .IsUnique();

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.ZipDistance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("DistanceMiles")
                        .HasPrecision(8, 2)
                        .HasColumnType("double");

                    b.Property<int>("EstimatedTravelTimeMinutes")
                        .HasColumnType("int");

                    b.Property<string>("FromZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ToZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("FromZipCode", "ToZipCode")
                        .IsUnique()
                        .HasDatabaseName("idx_zip_pair");

                    b.ToTable("ZipDistances");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DistanceMiles = 2.5,
                            EstimatedTravelTimeMinutes = 8,
                            FromZipCode = "12345",
                            IsVerified = true,
                            LastUpdated = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(5577),
                            ToZipCode = "12346"
                        },
                        new
                        {
                            Id = 2,
                            DistanceMiles = 5.0999999999999996,
                            EstimatedTravelTimeMinutes = 15,
                            FromZipCode = "12345",
                            IsVerified = true,
                            LastUpdated = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(5580),
                            ToZipCode = "12347"
                        },
                        new
                        {
                            Id = 3,
                            DistanceMiles = 2.5,
                            EstimatedTravelTimeMinutes = 8,
                            FromZipCode = "12346",
                            IsVerified = true,
                            LastUpdated = new DateTime(2025, 7, 17, 13, 25, 17, 503, DateTimeKind.Utc).AddTicks(5581),
                            ToZipCode = "12345"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryLink.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PantryLink.Core.Entities.DonationItem", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.FoodDonation", "FoodDonation")
                        .WithMany("Items")
                        .HasForeignKey("FoodDonationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodDonation");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.FoodDonation", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.Pantry", "Pantry")
                        .WithMany()
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.InventoryItem", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.Pantry", "Pantry")
                        .WithMany("Inventory")
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.MonetaryDonation", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.Pantry", "Pantry")
                        .WithMany()
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.PantryAdmin", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.Pantry", "Pantry")
                        .WithMany("Admins")
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryLink.Core.Entities.User", "User")
                        .WithMany("PantryAdmins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.PantryAnalytics", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.Pantry", "Pantry")
                        .WithMany()
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.PantryRating", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.Pantry", "Pantry")
                        .WithMany("Ratings")
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.UserPreference", b =>
                {
                    b.HasOne("PantryLink.Core.Entities.User", "User")
                        .WithMany("Preferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.FoodDonation", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.Pantry", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Inventory");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("PantryLink.Core.Entities.User", b =>
                {
                    b.Navigation("PantryAdmins");

                    b.Navigation("Preferences");
                });
#pragma warning restore 612, 618
        }
    }
}
