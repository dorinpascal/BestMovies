using BestMovies.Shared.Dtos.User;

namespace BestMovies.Shared.Dtos.Review;

public record ReviewDto(int MovieId, UserDto UserDto, int Rating, string? Comment);
