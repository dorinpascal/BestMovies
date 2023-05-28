namespace BestMovies.Shared.Dtos.Person;

public record PersonDetailsDto(int Id, string Name, string Biography, decimal AverageStarredMovieRanting, DateOnly? Birthday, IEnumerable<string>? StarredInMovies);