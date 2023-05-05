using System.Collections.Generic;
using System.Linq;
using BestMovies.Shared.Dtos.Actor;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace BestMovies.Bff.Extensions;

public static class ActorExtensions
{
    public static ActorDetailsDto ToDto(this Person actor, MovieCredits? credits) =>
        new(
            Id: actor.Id,
            Name: actor.Name,
            Biography: actor.Biography,
            Birthday: actor.Birthday,
            StarredInMovies: credits?.Cast.Select(m => m.Title)
        );
}