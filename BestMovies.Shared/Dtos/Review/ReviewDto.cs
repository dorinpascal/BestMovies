using BestMovies.Shared.Dtos.User;

namespace BestMovies.Shared.Dtos.Review;

public record ReviewDto(int MovieId, UserDto User, int Rating, string? Comment);
