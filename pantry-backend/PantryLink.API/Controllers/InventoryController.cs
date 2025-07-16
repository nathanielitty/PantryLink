using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;

namespace PantryLink.API.Controllers;

[ApiController]
[Route("api/pantries/{pantryId}/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;

    public InventoryController(IInventoryService inventoryService, IMapper mapper)
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all inventory items for a pantry
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventory(int pantryId)
    {
        var items = await _inventoryService.GetPantryInventoryAsync(pantryId);
        var itemDtos = _mapper.Map<IEnumerable<InventoryItemDto>>(items);
        
        return Ok(itemDtos);
    }

    /// <summary>
    /// Add new inventory item
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,PantryAdmin,Volunteer")]
    public async Task<ActionResult<InventoryItemDto>> AddInventoryItem(int pantryId, [FromBody] CreateInventoryItemDto createItemDto)
    {
        var item = _mapper.Map<InventoryItem>(createItemDto);
        item.PantryId = pantryId;
        
        var createdItem = await _inventoryService.AddInventoryItemAsync(item);
        var itemDto = _mapper.Map<InventoryItemDto>(createdItem);

        return CreatedAtAction(nameof(GetInventoryItem), new { pantryId, id = createdItem.Id }, itemDto);
    }

    /// <summary>
    /// Get specific inventory item
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<InventoryItemDto>> GetInventoryItem(int pantryId, int id)
    {
        var item = await _inventoryService.GetInventoryItemAsync(id);
        
        if (item == null || item.PantryId != pantryId)
        {
            return NotFound();
        }

        var itemDto = _mapper.Map<InventoryItemDto>(item);
        return Ok(itemDto);
    }

    /// <summary>
    /// Update inventory item
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,PantryAdmin,Volunteer")]
    public async Task<ActionResult<InventoryItemDto>> UpdateInventoryItem(int pantryId, int id, [FromBody] CreateInventoryItemDto updateItemDto)
    {
        var existingItem = await _inventoryService.GetInventoryItemAsync(id);
        
        if (existingItem == null || existingItem.PantryId != pantryId)
        {
            return NotFound();
        }

        _mapper.Map(updateItemDto, existingItem);
        var updatedItem = await _inventoryService.UpdateInventoryItemAsync(existingItem);
        var itemDto = _mapper.Map<InventoryItemDto>(updatedItem);

        return Ok(itemDto);
    }

    /// <summary>
    /// Update stock via barcode scanning
    /// </summary>
    [HttpPost("update-stock")]
    [Authorize(Roles = "Admin,PantryAdmin,Volunteer")]
    public async Task<ActionResult<InventoryItemDto>> UpdateStock(int pantryId, [FromBody] UpdateStockDto updateStockDto)
    {
        var item = await _inventoryService.UpdateStockByBarcodeAsync(pantryId, updateStockDto.Barcode, updateStockDto.QuantityChange);
        
        if (item == null)
        {
            return NotFound("Item with barcode not found in this pantry");
        }

        var itemDto = _mapper.Map<InventoryItemDto>(item);
        return Ok(itemDto);
    }

    /// <summary>
    /// Delete inventory item
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,PantryAdmin")]
    public async Task<IActionResult> DeleteInventoryItem(int pantryId, int id)
    {
        var item = await _inventoryService.GetInventoryItemAsync(id);
        
        if (item == null || item.PantryId != pantryId)
        {
            return NotFound();
        }

        await _inventoryService.DeleteInventoryItemAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Import inventory from CSV file
    /// </summary>
    [HttpPost("import/csv")]
    [Authorize(Roles = "Admin,PantryAdmin")]
    public async Task<IActionResult> ImportFromCsv(int pantryId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("File must be a CSV");
        }

        using var stream = file.OpenReadStream();
        await _inventoryService.ImportInventoryFromCsvAsync(pantryId, stream);

        return Ok("Inventory imported successfully");
    }

    /// <summary>
    /// Import inventory from JSON (e.g., Feeding America API)
    /// </summary>
    [HttpPost("import/json")]
    [Authorize(Roles = "Admin,PantryAdmin")]
    public async Task<IActionResult> ImportFromJson(int pantryId, [FromBody] string jsonData)
    {
        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return BadRequest("JSON data is required");
        }

        try
        {
            await _inventoryService.ImportInventoryFromJsonAsync(pantryId, jsonData);
            return Ok("Inventory imported successfully");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to import inventory: {ex.Message}");
        }
    }
}
