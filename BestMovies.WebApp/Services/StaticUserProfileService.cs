using BestMovies.WebApp.Models;

namespace BestMovies.WebApp.Services;

public class StaticUserProfileService : IUserProfileService
{
    public async Task<UserSettings> GetUserSettings()
    {
        var settings = new UserSettings();

        return await Task.FromResult(settings);
    }

    public Task SaveSettings(UserSettings _)
    {
        return Task.CompletedTask;
    }
}