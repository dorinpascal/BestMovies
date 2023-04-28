using BestMovies.WebApp.Models;

namespace BestMovies.WebApp.Services;

public interface IUserProfileService
{
    Task<UserSettings> GetUserSettings();

    Task SaveSettings(UserSettings userSettings);
}