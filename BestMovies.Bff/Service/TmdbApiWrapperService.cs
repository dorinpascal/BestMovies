using BestMovies.Bff.Extensions;
using BestMovies.Bff.Interface;
using BestMovies.Shared.Dtos.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;

namespace BestMovies.Bff.Service;

public class TmdbApiWrapperService : ITmdbApiWrapper
{
    private readonly TMDbClient _tmDbClient;
    public TmdbApiWrapperService(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public async Task<byte[]> GetImageBytes(string size, int id)
    {
        var config = await _tmDbClient.GetConfigAsync();
        if (!config.Images.BackdropSizes.Contains(size))
        {
            throw new ArgumentException($"Please provide a valid size. Available sizes: {string.Join(",", config.Images.BackdropSizes)}");
        }

        var movieImagePaths = await _tmDbClient.GetMovieImagesAsync(id);

        var bestImage = movieImagePaths?.Backdrops.MaxBy(x => x.VoteAverage);
        if (bestImage is null)
        {
            throw new NotFoundException(new TMDbStatusMessage()
            {
                StatusCode = 404,
                StatusMessage = $"Can not find image for the movie with id {id}"
            });
        }

        return await _tmDbClient.GetImageBytesAsync(size, bestImage.FilePath);
    }

    public async Task<IEnumerable<SearchMovieDto>> GetPopularMovies(string? language = null, string? region = null)
    {
        var searchContainer = await _tmDbClient.GetMoviePopularListAsync(language: language, region:region);
        var genres = await _tmDbClient.GetMovieGenresAsync();
        return searchContainer.Results.Select(m => m.ToDto(genres));
    }

    public async Task<IEnumerable<SearchMovieDto>> SearchMovie(string movieTitle)
    {
        var searchedMovies = await _tmDbClient.SearchMovieAsync(movieTitle);
        var genres = await _tmDbClient.GetMovieGenresAsync();
        return  searchedMovies.Results.Select(m => m.ToDto(genres));
    }
}
