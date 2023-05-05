using System;
using System.Linq;
using BestMovies.Shared.Dtos.Actor;
using TMDbLib.Objects.People;

namespace BestMovies.Bff.Extensions;

public static class ActorExtensions
{
    public static ActorDetailsDto ToDto(this Person actor, MovieCredits? credits) =>
        new(
            Id: actor.Id,
            Name: actor.Name,
            Biography: actor.Biography,
            Birthday: actor.Birthday is null ? null : DateOnly.FromDateTime(actor.Birthday.Value),
            StarredInMovies: credits?.Cast.Select(m => m.Title)
        );
}