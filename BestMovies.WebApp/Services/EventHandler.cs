
namespace BestMovies.WebApp.Services;

public class EventHandler
{
    public event Action? OnChange;
    
    public void NotifyStateChanged() => OnChange?.Invoke();
}