using AutoMapper;
using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;

namespace PantryLink.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Pantry mappings
        CreateMap<Pantry, PantryDto>()
            .ForMember(dest => dest.TotalInventoryItems, 
                       opt => opt.MapFrom(src => src.Inventory.Count));
        
        CreateMap<CreatePantryDto, Pantry>();

        // InventoryItem mappings
        CreateMap<InventoryItem, InventoryItemDto>();
        CreateMap<CreateInventoryItemDto, InventoryItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateAdded, opt => opt.Ignore())
            .ForMember(dest => dest.LastUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.Pantry, opt => opt.Ignore());

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Preferences, 
                       opt => opt.MapFrom(src => src.Preferences.Select(p => p.Preference)));

        // UserPreference mappings
        CreateMap<UserPreference, DietaryPreference>()
            .ConvertUsing(src => src.Preference);
    }
}

// Additional DTO for user profile
public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public IEnumerable<DietaryPreference> Preferences { get; set; } = new List<DietaryPreference>();
}
