using BestMovies.Bff.Extensions;
using BestMovies.Bff.Interface;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;

namespace BestMovies.Bff.Service;

public class MovieService : IMovieService
{
    private readonly TMDbClient _tmDbClient;
    public MovieService(TMDbClient tmDbClient)
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
            throw new NotFoundException();
        }

        return await _tmDbClient.GetImageBytesAsync(size, bestImage.FilePath);
    }

    public async Task<IEnumerable<SearchMovieDto>> GetPopularMovies(string? genre =null, string? language = null, string? region = null)
    {
        var genres = await _tmDbClient.GetMovieGenresAsync();
        IEnumerable<SearchMovieDto>? moviesDtos;

        if (genre is not null)
        {
            var searchedGenre = genres.Find(g => g.Name.Equals(genre, StringComparison.InvariantCultureIgnoreCase));

            if (searchedGenre is null)
            {
                throw new NotFoundException();
            }

            moviesDtos = await GetPopularMoviesByGenre(genres, searchedGenre, region, language);
        }
        else
        {
            var searchContainer = await _tmDbClient.GetMoviePopularListAsync(language: language, region: region);
            genres = await _tmDbClient.GetMovieGenresAsync();
            moviesDtos = searchContainer.Results.Select(m => m.ToDto(genres));
        }
        return moviesDtos;
    }

    public async Task<IEnumerable<SearchMovieDto>> SearchMovie(string movieTitle)
    {
        var searchedMovies = await _tmDbClient.SearchMovieAsync(movieTitle);
        var genres = await _tmDbClient.GetMovieGenresAsync();
        return  searchedMovies.Results.Select(m => m.ToDto(genres));
    }

    private async Task<IEnumerable<SearchMovieDto>> GetPopularMoviesByGenre(
        IEnumerable<Genre> allGenres,
        Genre genre,
        string? region,
        string? language)
    {
        var searchedMovies = await _tmDbClient.DiscoverMoviesAsync()
            .IncludeWithAllOfGenre(new[] { genre })
            .WhereReleaseDateIsInRegion(region)
            .WhereLanguageIs(language)
            .Query();
        var moviesDtos = searchedMovies.Results.Select(m => m.ToDto(allGenres));

        return moviesDtos;
    }

}
