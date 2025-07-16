using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;

namespace PantryLink.Core.Interfaces;

public interface IPantryService
{
    Task<IEnumerable<Pantry>> GetPantriesByZipCodeAsync(string zipCode);
    Task<Pantry?> GetPantryByIdAsync(int id);
    Task<Pantry> CreatePantryAsync(Pantry pantry);
    Task<Pantry> UpdatePantryAsync(Pantry pantry);
    Task DeletePantryAsync(int id);
    Task<IEnumerable<Pantry>> GetRecommendedPantriesAsync(int userId, string zipCode);
}

public interface IInventoryService
{
    Task<InventoryItem?> GetInventoryItemAsync(int id);
    Task<IEnumerable<InventoryItem>> GetPantryInventoryAsync(int pantryId);
    Task<InventoryItem> AddInventoryItemAsync(InventoryItem item);
    Task<InventoryItem> UpdateInventoryItemAsync(InventoryItem item);
    Task DeleteInventoryItemAsync(int id);
    Task<InventoryItem?> UpdateStockByBarcodeAsync(int pantryId, string barcode, int quantityChange);
    Task<IEnumerable<InventoryItem>> SearchItemsAsync(string query);
    Task ImportInventoryFromCsvAsync(int pantryId, Stream csvStream);
    Task ImportInventoryFromJsonAsync(int pantryId, string jsonData);
}

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User> UpdateUserPreferencesAsync(int userId, IEnumerable<DietaryPreference> preferences);
    Task<IEnumerable<DietaryPreference>> GetUserPreferencesAsync(int userId);
}

public interface IAnalyticsService
{
    Task<SystemAnalytics> GetSystemAnalyticsAsync(DateTime? date = null);
    Task<PantryAnalytics> GetPantryAnalyticsAsync(int pantryId, DateTime? date = null);
    Task<IEnumerable<PantryAnalytics>> GetTopPerformingPantriesAsync(int count = 10);
    Task<AnalyticsDashboardDto> GetDashboardDataAsync();
    Task UpdatePantryAnalyticsAsync(int pantryId);
    Task UpdateSystemAnalyticsAsync();
    Task<Dictionary<string, int>> GetPopularCategoriesAsync();
    Task<Dictionary<string, int>> GetSearchesByZipCodeAsync();
}

public interface IZipDistanceService
{
    Task<ZipDistance?> GetDistanceAsync(string fromZip, string toZip);
    Task<IEnumerable<ZipDistance>> GetNearbyZipCodesAsync(string zipCode, double maxDistance);
    Task<ZipDistance> AddOrUpdateDistanceAsync(string fromZip, string toZip, double distanceMiles, int travelTimeMinutes);
    Task<IEnumerable<ZipDistance>> GetAllDistancesForZipAsync(string zipCode);
    Task<double> CalculateDistanceAsync(string fromZip, string toZip);
}
