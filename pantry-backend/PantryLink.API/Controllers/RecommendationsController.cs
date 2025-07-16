using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryLink.Core.DTOs;
using PantryLink.Core.Interfaces;

namespace PantryLink.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecommendationsController : ControllerBase
{
    private readonly IPantryService _pantryService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public RecommendationsController(IPantryService pantryService, IUserService userService, IMapper mapper)
    {
        _pantryService = pantryService;
        _userService = userService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get AI-curated pantry recommendations based on user preferences and location
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PantryRecommendationDto>>> GetRecommendations([FromQuery] string zipCode)
    {
        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length != 5)
        {
            return BadRequest("Valid 5-digit ZIP code is required");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var recommendedPantries = await _pantryService.GetRecommendedPantriesAsync(userId, zipCode);
        var userPreferences = await _userService.GetUserPreferencesAsync(userId);

        var recommendations = recommendedPantries.Select(pantry =>
        {
            var pantryDto = _mapper.Map<PantryDto>(pantry);

            // Filter recommended items based on user preferences
            var recommendedItems = pantry.Inventory.Where(item =>
            {
                if (!userPreferences.Any()) return true;

                return userPreferences.Any(pref => pref switch
                {
                    Core.Entities.DietaryPreference.Vegan => item.IsVegan,
                    Core.Entities.DietaryPreference.GlutenFree => item.IsGlutenFree,
                    Core.Entities.DietaryPreference.Halal => item.IsHalal,
                    Core.Entities.DietaryPreference.Kosher => item.IsKosher,
                    _ => true
                });
            }).OrderByDescending(item => item.Priority).Take(5);

            var recommendedItemDtos = _mapper.Map<IEnumerable<InventoryItemDto>>(recommendedItems);

            // Calculate recommendation score
            var score = CalculateRecommendationScore(pantry, userPreferences);

            // Generate reason for recommendation
            var reason = GenerateRecommendationReason(pantry, userPreferences, recommendedItems);

            return new PantryRecommendationDto
            {
                Pantry = pantryDto,
                RecommendedItems = recommendedItemDtos,
                Score = score,
                ReasonForRecommendation = reason
            };
        }).ToList();

        return Ok(recommendations);
    }

    private static int CalculateRecommendationScore(Core.Entities.Pantry pantry, IEnumerable<Core.Entities.DietaryPreference> preferences)
    {
        var score = 0;

        // Base score from pantry rating (0-50 points)
        score += (int)(pantry.AverageRating * 10);

        // Preference matching score (0-30 points)
        if (preferences.Any())
        {
            var matchingItems = pantry.Inventory.Count(item =>
                preferences.Any(pref => pref switch
                {
                    Core.Entities.DietaryPreference.Vegan => item.IsVegan,
                    Core.Entities.DietaryPreference.GlutenFree => item.IsGlutenFree,
                    Core.Entities.DietaryPreference.Halal => item.IsHalal,
                    Core.Entities.DietaryPreference.Kosher => item.IsKosher,
                    _ => true
                })
            );

            score += Math.Min(matchingItems * 2, 30);
        }

        // Stock freshness score (0-20 points)
        var soonToExpireItems = pantry.Inventory.Count(item =>
            item.ExpirationDate.HasValue && (item.ExpirationDate.Value - DateTime.UtcNow).Days <= 7);
        score += Math.Min(soonToExpireItems * 3, 20);

        return score;
    }

    private static string GenerateRecommendationReason(Core.Entities.Pantry pantry, IEnumerable<Core.Entities.DietaryPreference> preferences, IEnumerable<Core.Entities.InventoryItem> recommendedItems)
    {
        var reasons = new List<string>();

        if (pantry.AverageRating >= 4.0)
        {
            reasons.Add($"Highly rated pantry ({pantry.AverageRating:F1}/5.0 stars)");
        }

        if (preferences.Any())
        {
            var matchingItems = recommendedItems.Count(item =>
                preferences.Any(pref => pref switch
                {
                    Core.Entities.DietaryPreference.Vegan => item.IsVegan,
                    Core.Entities.DietaryPreference.GlutenFree => item.IsGlutenFree,
                    Core.Entities.DietaryPreference.Halal => item.IsHalal,
                    Core.Entities.DietaryPreference.Kosher => item.IsKosher,
                    _ => true
                })
            );

            if (matchingItems > 0)
            {
                var preferenceNames = string.Join(", ", preferences.Select(p => p.ToString()));
                reasons.Add($"Has {matchingItems} items matching your {preferenceNames} preferences");
            }
        }

        var soonToExpireItems = recommendedItems.Count(item =>
            item.ExpirationDate.HasValue && (item.ExpirationDate.Value - DateTime.UtcNow).Days <= 7);

        if (soonToExpireItems > 0)
        {
            reasons.Add($"Has {soonToExpireItems} items expiring soon and needing immediate distribution");
        }

        if (!reasons.Any())
        {
            reasons.Add("Good availability of fresh items");
        }

        return string.Join(". ", reasons) + ".";
    }
}
