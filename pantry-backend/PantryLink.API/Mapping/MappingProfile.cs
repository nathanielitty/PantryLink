using AutoMapper;
using PantryLink.Core.DTOs;
using PantryLink.Core.Entities;

namespace PantryLink.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Pantry mappings
        CreateMap<Pantry, PantryDto>()
            .ForMember(dest => dest.TotalInventoryItems, opt => opt.MapFrom(src => src.Inventory.Count));
        
        CreateMap<CreatePantryDto, Pantry>();

        // InventoryItem mappings
        CreateMap<InventoryItem, InventoryItemDto>();
        CreateMap<CreateInventoryItemDto, InventoryItem>();

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Preferences, opt => opt.MapFrom(src => src.Preferences.Select(p => p.Preference)));

        CreateMap<UserPreference, DietaryPreference>()
            .ConvertUsing(src => src.Preference);
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<DietaryPreference> Preferences { get; set; } = new List<DietaryPreference>();
}
