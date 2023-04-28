namespace BestMovies.WebApp.Models;

public class UserSettings
{
    public bool DarkMode { get; init; } = false;
    public string? Region { get; init; }
    public string? Language { get; init; }
}