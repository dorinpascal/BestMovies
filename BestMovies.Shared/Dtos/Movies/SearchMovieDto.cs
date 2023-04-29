namespace BestMovies.Shared.Dtos.Movies;

public record SearchMovieDto(int Id, string Title, IEnumerable<string> Genres);