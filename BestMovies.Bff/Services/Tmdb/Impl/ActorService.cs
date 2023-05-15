using System;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Actor;
using TMDbLib.Client;

namespace BestMovies.Bff.Services.Tmdb.Impl;

public class ActorService : IActorService
{
    private readonly ITMDbWrapperService _tmDbClient;

    public ActorService(ITMDbWrapperService tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public async Task<ActorDetailsDto> GetActorDetails(int id)
    {
        var person = await _tmDbClient.GetPersonAsync(id);
        if (person is null)
        {
            throw new NotFoundException($"Cannot find any actor with id '{id}'");
        }

        var movieCredits = await _tmDbClient.GetPersonMovieCreditsAsync(id);
        return person.ToDto(movieCredits);
    }

    public async Task<byte[]> GetImageBytes(string size, int id)
    {
        var config = await _tmDbClient.GetConfigAsync();
        if (!config.Images.BackdropSizes.Contains(size))
        {
            throw new ArgumentException($"Please provide a valid size. Available sizes: {string.Join(",", config.Images.BackdropSizes)}");
        }

        var profileImages = await _tmDbClient.GetPersonImagesAsync(id);

        var bestImage = profileImages.Profiles.MaxBy(x => x.VoteAverage);
        if (bestImage is null)
        {
            throw new NotFoundException($"Cannot find any image for the actor with id '{id}'");
        }

        return await _tmDbClient.GetImageBytesAsync(size, bestImage.FilePath);
    }
}