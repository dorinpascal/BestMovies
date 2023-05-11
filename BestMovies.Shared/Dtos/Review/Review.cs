namespace BestMovies.Shared.Dtos.Review;

public record Review(int Id, int UserId, int Rating, string Comment);
