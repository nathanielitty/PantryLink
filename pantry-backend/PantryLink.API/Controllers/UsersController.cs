using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;

namespace PantryLink.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get current user's profile
    /// </summary>
    [HttpGet("profile")]
    public async Task<ActionResult<User>> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _userService.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        // Remove sensitive information
        user.PasswordHash = null;
        user.SecurityStamp = null;

        return Ok(user);
    }

    /// <summary>
    /// Update user's dietary preferences
    /// </summary>
    [HttpPut("preferences")]
    public async Task<ActionResult<IEnumerable<DietaryPreference>>> UpdatePreferences([FromBody] UserPreferencesDto preferencesDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var updatedUser = await _userService.UpdateUserPreferencesAsync(userId, preferencesDto.Preferences);
        var preferences = await _userService.GetUserPreferencesAsync(userId);

        return Ok(preferences);
    }

    /// <summary>
    /// Get user's current dietary preferences
    /// </summary>
    [HttpGet("preferences")]
    public async Task<ActionResult<IEnumerable<DietaryPreference>>> GetPreferences()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var preferences = await _userService.GetUserPreferencesAsync(userId);
        return Ok(preferences);
    }
}
