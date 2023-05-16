using System;

namespace BestMovies.Api.Persistence.Entity;

public class Review
{
    public int Id { get; private set; }
    public string UserId { get; }
    public int Rating { get; }
    public string? Comment { get; }

    public Review(string userId, int rating, string? comment)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        Rating = rating;
        Comment = comment;
    }
}
