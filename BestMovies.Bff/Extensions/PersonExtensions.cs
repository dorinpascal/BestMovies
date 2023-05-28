using System;
using System.Linq;
using BestMovies.Shared.Dtos.Person;
using BestMovies.Shared.Dtos.Person.Actor;
using BestMovies.Shared.Dtos.Person.Director;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace BestMovies.Bff.Extensions;

public static class PersonExtensions
{
    public static PersonDetailsDto ToDto(this Person person, MovieCredits? credits, decimal averageStarredMovieRanting) =>
        new(
            Id: person.Id,
            Name: person.Name,
            Biography: person.Biography,
            AverageStarredMovieRanting: averageStarredMovieRanting,
            Birthday: person.Birthday is null ? null : DateOnly.FromDateTime(person.Birthday.Value),
            StarredInMovies: credits?.Cast.Select(m => m.Title)
        );

    public static ActorDto ToDto(this Cast actor) =>
        new(
            Id: actor.Id,
            Name: actor.Name,
            RoleName: actor.Character
        );

    public static DirectorDto ToDto(this Crew director) =>
        new(
            Id: director.Id,
            Name: director.Name);

}