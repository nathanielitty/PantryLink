using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PantryLink.Core.DTOs;
using PantryLink.Core.Interfaces;

namespace PantryLink.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;

    public SearchController(IInventoryService inventoryService, IMapper mapper)
    {
        _inventoryService = inventoryService;
        _mapper = mapper;
    }

    /// <summary>
    /// Search for inventory items across all pantries using Trie algorithm
    /// </summary>
    [HttpGet("items")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> SearchItems([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
        {
            return BadRequest("Search query must be at least 2 characters long");
        }

        var items = await _inventoryService.SearchItemsAsync(query);
        var itemDtos = _mapper.Map<IEnumerable<InventoryItemDto>>(items);
        
        return Ok(itemDtos);
    }
}
