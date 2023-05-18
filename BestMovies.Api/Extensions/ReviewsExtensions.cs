using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.Review;
using System.Collections.Generic;
using System.Linq;

namespace BestMovies.Api.Extensions;

public static class ReviewsExtensions
{
    public static ReviewDto ToDto(this Review review) =>
        new(
            MovieId: review.MovieId,
            UserDto: review.User.ToDto(),
            Rating: review.Rating,
            Comment: review.Comment);


    public static ReviewStatsDto ToDto(this IEnumerable<ReviewDto> reviewDtos)
    {
        var reviewListDto = new ReviewStatsDto(
            Reviews: reviewDtos.ToList(),
            AvgRating: reviewDtos.Any() ? (int)reviewDtos.Average(r => r.Rating) : 0,
            ReviewCount: reviewDtos.Count()
        );

        return reviewListDto;
    }
}

