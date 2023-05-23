namespace BestMovies.Shared.Dtos.Person;

public record PersonDetailsDto(int Id, string Name, string Biography, DateOnly? Birthday, IEnumerable<string>? StarredInMovies);