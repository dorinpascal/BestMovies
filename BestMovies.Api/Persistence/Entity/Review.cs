using System;

namespace BestMovies.Api.Persistence.Entity;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Review
{
    public string UserId { get; }
    public virtual User User { get; private set; } = null!;
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
}
