using BestMovies.Shared.Dtos.User;

namespace BestMovies.Shared.Dtos.Review;

public record ReviewDto(int MovieId, SimpleUserDto SimpleUser, int Rating, string? Comment);
