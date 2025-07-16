using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PantryLink.Core.Algorithms;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;
using PantryLink.Infrastructure.Data;

namespace PantryLink.Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ItemSearchTrie _searchTrie;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
        _searchTrie = new ItemSearchTrie();
        InitializeSearchTrie();
    }

    public async Task<InventoryItem?> GetInventoryItemAsync(int id)
    {
        return await _context.InventoryItems
            .Include(i => i.Pantry)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InventoryItem>> GetPantryInventoryAsync(int pantryId)
    {
        return await _context.InventoryItems
            .Where(i => i.PantryId == pantryId && i.Quantity > 0)
            .OrderBy(i => i.ExpirationDate)
            .ThenBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<InventoryItem> AddInventoryItemAsync(InventoryItem item)
    {
        item.DateAdded = DateTime.UtcNow;
        item.LastUpdated = DateTime.UtcNow;
        
        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();

        // Add to search trie
        _searchTrie.Insert(item.Name, item.Id);
        _searchTrie.Insert(item.Category, item.Id);

        return item;
    }

    public async Task<InventoryItem> UpdateInventoryItemAsync(InventoryItem item)
    {
        item.LastUpdated = DateTime.UtcNow;
        _context.InventoryItems.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task DeleteInventoryItemAsync(int id)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item != null)
        {
            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<InventoryItem?> UpdateStockByBarcodeAsync(int pantryId, string barcode, int quantityChange)
    {
        var item = await _context.InventoryItems
            .FirstOrDefaultAsync(i => i.PantryId == pantryId && i.Barcode == barcode);

        if (item != null)
        {
            var newQuantity = item.Quantity + quantityChange;
            item.Quantity = Math.Max(0, newQuantity); // Ensure quantity doesn't go negative
            item.LastUpdated = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }

        return item;
    }

    public async Task<IEnumerable<InventoryItem>> SearchItemsAsync(string query)
    {
        // Use Trie for fast prefix search
        var itemIds = _searchTrie.Search(query);
        
        if (itemIds.Any())
        {
            return await _context.InventoryItems
                .Where(i => itemIds.Contains(i.Id) && i.Quantity > 0)
                .Include(i => i.Pantry)
                .OrderBy(i => i.ExpirationDate)
                .ToListAsync();
        }

        // Fallback to database LIKE search
        return await _context.InventoryItems
            .Where(i => (i.Name.Contains(query) || i.Category.Contains(query)) && i.Quantity > 0)
            .Include(i => i.Pantry)
            .OrderBy(i => i.ExpirationDate)
            .ToListAsync();
    }

    public async Task ImportInventoryFromCsvAsync(int pantryId, Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<CsvInventoryRecord>();

        foreach (var record in records)
        {
            var item = new InventoryItem
            {
                PantryId = pantryId,
                Name = record.Name,
                Description = record.Description ?? string.Empty,
                Barcode = record.Barcode,
                Quantity = record.Quantity,
                Unit = record.Unit ?? "units",
                ExpirationDate = record.ExpirationDate,
                Category = record.Category ?? "General",
                UsdaFoodId = record.UsdaFoodId,
                IsVegan = record.IsVegan,
                IsGlutenFree = record.IsGlutenFree,
                IsHalal = record.IsHalal,
                IsKosher = record.IsKosher
            };

            await AddInventoryItemAsync(item);
        }
    }

    public async Task ImportInventoryFromJsonAsync(int pantryId, string jsonData)
    {
        var records = JsonConvert.DeserializeObject<List<JsonInventoryRecord>>(jsonData);
        
        if (records != null)
        {
            foreach (var record in records)
            {
                var item = new InventoryItem
                {
                    PantryId = pantryId,
                    Name = record.Name,
                    Description = record.Description ?? string.Empty,
                    Barcode = record.Barcode,
                    Quantity = record.Quantity,
                    Unit = record.Unit ?? "units",
                    ExpirationDate = record.ExpirationDate,
                    Category = record.Category ?? "General",
                    UsdaFoodId = record.UsdaFoodId,
                    IsVegan = record.IsVegan,
                    IsGlutenFree = record.IsGlutenFree,
                    IsHalal = record.IsHalal,
                    IsKosher = record.IsKosher
                };

                await AddInventoryItemAsync(item);
            }
        }
    }

    private void InitializeSearchTrie()
    {
        // This would be called on startup to populate the trie with existing items
        Task.Run(async () =>
        {
            var items = await _context.InventoryItems.ToListAsync();
            foreach (var item in items)
            {
                _searchTrie.Insert(item.Name, item.Id);
                _searchTrie.Insert(item.Category, item.Id);
            }
        });
    }

    // Helper classes for CSV/JSON import
    private class CsvInventoryRecord
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public int Quantity { get; set; }
        public string? Unit { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Category { get; set; }
        public string? UsdaFoodId { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsHalal { get; set; }
        public bool IsKosher { get; set; }
    }

    private class JsonInventoryRecord
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public int Quantity { get; set; }
        public string? Unit { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Category { get; set; }
        public string? UsdaFoodId { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsHalal { get; set; }
        public bool IsKosher { get; set; }
    }
}
