namespace BestMovies.Shared.Dtos.Actor;

public record ActorDetailsDto(int Id, string Name, string Biography, DateOnly? Birthday, IEnumerable<string>? StarredInMovies);