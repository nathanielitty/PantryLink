using Microsoft.EntityFrameworkCore;
using PantryLink.Core.Entities;
using PantryLink.Core.Interfaces;
using PantryLink.Infrastructure.Data;

namespace PantryLink.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.Preferences)
            .Include(u => u.PantryAdmins)
            .ThenInclude(pa => pa.Pantry)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> UpdateUserPreferencesAsync(int userId, IEnumerable<DietaryPreference> preferences)
    {
        var user = await _context.Users
            .Include(u => u.Preferences)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        // Remove existing preferences
        _context.UserPreferences.RemoveRange(user.Preferences);

        // Add new preferences
        foreach (var preference in preferences)
        {
            user.Preferences.Add(new UserPreference
            {
                UserId = userId,
                Preference = preference,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<DietaryPreference>> GetUserPreferencesAsync(int userId)
    {
        return await _context.UserPreferences
            .Where(up => up.UserId == userId)
            .Select(up => up.Preference)
            .ToListAsync();
    }
}
