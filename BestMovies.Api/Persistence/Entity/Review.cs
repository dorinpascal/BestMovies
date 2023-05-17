using System;

namespace BestMovies.Api.Persistence.Entity;

public class Review
{
    public string UserId { get; }
    public virtual User User { get; } = null!;
    public int MovieId { get; }
    public int Rating { get; }
    public string? Comment { get; }

    public Review(int movieId, string userId, int rating, string? comment)
    {
        MovieId = movieId;
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        Rating = rating;
        Comment = comment;
    }
    
#pragma warning disable CS8618
    public Review()
    {
        // Needed by EF Core
    }
#pragma warning restore CS8618
}
