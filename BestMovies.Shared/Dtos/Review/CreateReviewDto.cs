namespace BestMovies.Shared.Dtos.Review;

public record CreateReviewDto(int MovieId, int Rating, string? Comment);