using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.Review;

namespace BestMovies.Api.Extensions;

public static class ReviewsExtensions
{
    public static ReviewDto ToDto(this Review review) =>
        new(
            MovieId: review.MovieId,
            UserDto: review.User.ToDto(),
            Rating: review.Rating,
            Comment: review.Comment);
}