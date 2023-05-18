namespace BestMovies.Shared.Dtos.Review;

public record ReviewListDto(IEnumerable<ReviewDto> Reviews, int AvgRating, int ReviewCount );
