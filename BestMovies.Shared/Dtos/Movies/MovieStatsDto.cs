namespace BestMovies.Shared.Dtos.Movies;

public record MovieStatsDto(decimal AverageRating, int ReviewsCount, int Watched, int WantToWatch);
