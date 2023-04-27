namespace BestMovies.Shared.Dtos.Movies;

public record SearchMovieDto(int Id, string Title, string? PosterPath, IEnumerable<string> Genres);