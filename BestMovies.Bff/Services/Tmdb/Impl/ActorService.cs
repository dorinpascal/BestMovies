using System;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Person;

namespace BestMovies.Bff.Services.Tmdb.Impl;

public class ActorService : IActorService
{
    private readonly ITMDbWrapperService _tmDbClient;
    private readonly IBestMoviesApiClient _bestMoviesApiClient;

    public ActorService(ITMDbWrapperService tmDbClient, IBestMoviesApiClient bestMoviesApiClient)
    {
        _tmDbClient = tmDbClient;
        _bestMoviesApiClient = bestMoviesApiClient;
    }

    public async Task<PersonDetailsDto> GetActorDetails(int id)
    {
        var person = await _tmDbClient.GetPersonAsync(id);
        if (person is null)
        {
            throw new NotFoundException($"Cannot find any actor with id '{id}'");
        }
        
        var movieCredits = await _tmDbClient.GetPersonMovieCreditsAsync(id);

        var starredMovieIds = movieCredits.Cast.Select(m => m.Id);

        var avgStarredMovieRating = decimal.Zero;
        try
        {
            avgStarredMovieRating = await _bestMoviesApiClient.GetAverageRatingOfMovies(starredMovieIds);
        }
        catch (Exception)
        {
            // ignored
        }

        return person.ToDto(movieCredits, avgStarredMovieRating);
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