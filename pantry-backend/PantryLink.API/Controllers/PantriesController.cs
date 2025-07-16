using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;

namespace PantryLink.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PantriesController : ControllerBase
{
    private readonly IPantryService _pantryService;
    private readonly IMapper _mapper;

    public PantriesController(IPantryService pantryService, IMapper mapper)
    {
        _pantryService = pantryService;
        _mapper = mapper;
    }

    /// <summary>
    /// Search pantries by ZIP code
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PantryDto>>> GetPantries([FromQuery] string zip)
    {
        if (string.IsNullOrWhiteSpace(zip) || zip.Length != 5)
        {
            return BadRequest("Valid 5-digit ZIP code is required");
        }

        var pantries = await _pantryService.GetPantriesByZipCodeAsync(zip);
        var pantryDtos = _mapper.Map<IEnumerable<PantryDto>>(pantries);
        
        return Ok(pantryDtos);
    }

    /// <summary>
    /// Get pantry by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PantryDto>> GetPantry(int id)
    {
        var pantry = await _pantryService.GetPantryByIdAsync(id);
        
        if (pantry == null)
        {
            return NotFound();
        }

        var pantryDto = _mapper.Map<PantryDto>(pantry);
        return Ok(pantryDto);
    }

    /// <summary>
    /// Create a new pantry
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PantryDto>> CreatePantry([FromBody] CreatePantryDto createPantryDto)
    {
        var pantry = _mapper.Map<Pantry>(createPantryDto);
        var createdPantry = await _pantryService.CreatePantryAsync(pantry);
        var pantryDto = _mapper.Map<PantryDto>(createdPantry);

        return CreatedAtAction(nameof(GetPantry), new { id = createdPantry.Id }, pantryDto);
    }

    /// <summary>
    /// Update pantry information
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,PantryAdmin")]
    public async Task<ActionResult<PantryDto>> UpdatePantry(int id, [FromBody] CreatePantryDto updatePantryDto)
    {
        var existingPantry = await _pantryService.GetPantryByIdAsync(id);
        
        if (existingPantry == null)
        {
            return NotFound();
        }

        _mapper.Map(updatePantryDto, existingPantry);
        var updatedPantry = await _pantryService.UpdatePantryAsync(existingPantry);
        var pantryDto = _mapper.Map<PantryDto>(updatedPantry);

        return Ok(pantryDto);
    }

    /// <summary>
    /// Delete (deactivate) a pantry
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePantry(int id)
    {
        await _pantryService.DeletePantryAsync(id);
        return NoContent();
    }
}
