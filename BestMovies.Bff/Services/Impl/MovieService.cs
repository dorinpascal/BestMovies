using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace BestMovies.Bff.Services.Impl;

public class MovieService : IMovieService
{
    private readonly ITMDbWrapperService _tmDbClient;
    public MovieService(ITMDbWrapperService tmDbClient)
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
            throw new NotFoundException($"Cannot find any image for the movie with id '{id}'");
        }

        return await _tmDbClient.GetImageBytesAsync(size, bestImage.FilePath);
    }

    public async Task<MovieDetailsDto> GetMovieDetails(int id)
    {     
        var searchContainer = await _tmDbClient.GetMovieAsync(id);
        if (searchContainer is null)
        {
            throw new NotFoundException($"No movies found with the id '{id}'");
        }
        
        var credits = await _tmDbClient.GetMovieCreditsAsync(id);
            
        var movieDetailsDto = searchContainer.ToDto(credits.Cast.Take(5));
        return movieDetailsDto;
    }

    public async Task<IEnumerable<SearchMovieDto>> GetPopularMovies(string? genre =null, string? language = null, string? region = null)
    {
        var genres = await _tmDbClient.GetMovieGenresAsync();

        var searchContainer = genre is null
            ? await _tmDbClient.GetMoviePopularListAsync(language: language, region: region)
            : await GetPopularMoviesByGenre(genres, genre, region, language);
        
        return searchContainer.Results.Select(m => m.ToDto(genres));
    }

    public async Task<IEnumerable<SearchMovieDto>> SearchMovie(string movieTitle)
    {
        var searchedMovies = await _tmDbClient.SearchMovieAsync(movieTitle);
        var genres = await _tmDbClient.GetMovieGenresAsync();
        return  searchedMovies.Results.Select(m => m.ToDto(genres));
    }
    
    private async Task<SearchContainer<SearchMovie>> GetPopularMoviesByGenre(IEnumerable<Genre> genres, string genre, string? region, string? language)
    {
        var searchedGenre = genres.FirstOrDefault(g => g.Name.Equals(genre, StringComparison.InvariantCultureIgnoreCase));
        if (searchedGenre is null)
        {
            throw new NotFoundException($"Cannot find any movie with the genre '{genre}'");
        }

        return await _tmDbClient.GetMoviePopularListByGenreAsync(searchedGenre, language, region);
    }

}
