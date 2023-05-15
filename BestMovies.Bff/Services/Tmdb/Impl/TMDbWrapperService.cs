using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Certifications;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Configuration;
using TMDbLib.Objects.Credit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Timezones;
using TMDbLib.Objects.Trending;
using TMDbLib.Objects.TvShows;
using Country = TMDbLib.Objects.Countries.Country;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace BestMovies.Bff.Services.Tmdb.Impl;

// ReSharper disable once InconsistentNaming
public class TMDbWrapperService : ITMDbWrapperService
{
    private readonly TMDbClient _tmDbClient;

    public TMDbWrapperService(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public Task<TMDbConfig> GetConfigAsync()
    {
        return _tmDbClient.GetConfigAsync();
    }

    public Task<byte[]> GetImageBytesAsync(string size, string filePath, bool useSsl = false,
        CancellationToken token = new())
    {
        return _tmDbClient.GetImageBytesAsync(size, filePath, useSsl, token);
    }

    public Task<SearchContainer<SearchMovie>> GetMoviePopularListByGenreAsync(Genre searchedGenre, string? region, string? language)
    {
        return _tmDbClient.DiscoverMoviesAsync()
            .IncludeWithAllOfGenre(new[] { searchedGenre })
            .WhereReleaseDateIsInRegion(region)
            .WhereLanguageIs(language)
            .Query();
    }

    public DiscoverMovie DiscoverMoviesAsync()
    {
        return _tmDbClient.DiscoverMoviesAsync();
    }

    public Task<List<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetMovieGenresAsync(cancellationToken);
    }

    public Task<Movie> GetMovieAsync(int movieId, MovieMethods extraMethods = MovieMethods.Undefined,
        CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetMovieAsync(movieId, extraMethods, cancellationToken);
    }

    public Task<Credits> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetMovieCreditsAsync(movieId, cancellationToken);
    }

    public Task<ImagesWithId> GetMovieImagesAsync(int movieId, CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetMovieImagesAsync(movieId, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMoviePopularListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetMoviePopularListAsync(language, page, region, cancellationToken);
    }

    public Task<Person> GetPersonAsync(int personId, PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetPersonAsync(personId, extraMethods, cancellationToken);
    }

    public Task<ProfileImages> GetPersonImagesAsync(int personId, CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetPersonImagesAsync(personId, cancellationToken);
    }

    public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, CancellationToken cancellationToken = new())
    {
        return _tmDbClient.GetPersonMovieCreditsAsync(personId, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null,
        int primaryReleaseYear = 0, CancellationToken cancellationToken = new())
    {
        return _tmDbClient.SearchMovieAsync(query, page, includeAdult, year, region, primaryReleaseYear,
            cancellationToken);
    }
}