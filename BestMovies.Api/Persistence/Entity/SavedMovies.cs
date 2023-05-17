using System;

namespace BestMovies.Api.Persistence.Entity;

public class SavedMovies
{
    public string UserId { get; }

    public int MovieId { get; }

    public bool IsWatched { get; }

    public SavedMovies(string? userId, int movieId, bool isWatched)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        MovieId = movieId;
        IsWatched = isWatched;
    }
    
}