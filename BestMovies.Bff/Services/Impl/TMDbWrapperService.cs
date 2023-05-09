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

namespace BestMovies.Bff.Services.Impl;

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

    public Uri GetImageUrl(string size, string filePath, bool useSsl = false)
    {
        return _tmDbClient.GetImageUrl(size, filePath, useSsl);
    }

    public Task<byte[]> GetImageBytesAsync(string size, string filePath, bool useSsl = false,
        CancellationToken token = new CancellationToken())
    {
        return _tmDbClient.GetImageBytesAsync(size, filePath, useSsl, token);
    }

    public void SetConfig(TMDbConfig config)
    {
        _tmDbClient.SetConfig(config);
    }

    public Task SetSessionInformationAsync(string sessionId, SessionType sessionType)
    {
        return _tmDbClient.SetSessionInformationAsync(sessionId, sessionType);
    }
    
    public Task<bool> AccountChangeFavoriteStatusAsync(MediaType mediaType, int mediaId, bool isFavorite,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountChangeFavoriteStatusAsync(mediaType, mediaId, isFavorite, cancellationToken);
    }

    public Task<bool> AccountChangeWatchlistStatusAsync(MediaType mediaType, int mediaId, bool isOnWatchlist,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountChangeWatchlistStatusAsync(mediaType, mediaId, isOnWatchlist, cancellationToken);
    }

    public Task<AccountDetails> AccountGetDetailsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetDetailsAsync(cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> AccountGetFavoriteMoviesAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetFavoriteMoviesAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> AccountGetFavoriteTvAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetFavoriteTvAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<SearchContainer<AccountList>> AccountGetListsAsync(int page = 1, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetListsAsync(page, language, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> AccountGetMovieWatchlistAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetMovieWatchlistAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<SearchContainer<SearchMovieWithRating>> AccountGetRatedMoviesAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetRatedMoviesAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<SearchContainer<AccountSearchTvEpisode>> AccountGetRatedTvShowEpisodesAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetRatedTvShowEpisodesAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<SearchContainer<AccountSearchTv>> AccountGetRatedTvShowsAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetRatedTvShowsAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> AccountGetTvWatchlistAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AccountGetTvWatchlistAsync(page, sortBy, sortOrder, language, cancellationToken);
    }

    public Task<GuestSession> AuthenticationCreateGuestSessionAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AuthenticationCreateGuestSessionAsync(cancellationToken);
    }

    public Task<UserSession> AuthenticationGetUserSessionAsync(string initialRequestToken,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AuthenticationGetUserSessionAsync(initialRequestToken, cancellationToken);
    }

    public Task<UserSession> AuthenticationGetUserSessionAsync(string username, string password,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AuthenticationGetUserSessionAsync(username, password, cancellationToken);
    }

    public Task<Token> AuthenticationRequestAuthenticationTokenAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AuthenticationRequestAutenticationTokenAsync(cancellationToken);
    }

    public Task AuthenticationValidateUserTokenAsync(string initialRequestToken, string username, string password,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.AuthenticationValidateUserTokenAsync(initialRequestToken, username, password, cancellationToken);
    }

    public Task<CertificationsContainer> GetMovieCertificationsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieCertificationsAsync(cancellationToken);
    }

    public Task<CertificationsContainer> GetTvCertificationsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvCertificationsAsync(cancellationToken);
    }

    public Task<SearchContainer<ChangesListItem>> GetMoviesChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMoviesChangesAsync(page, startDate, endDate, cancellationToken);
    }

    public Task<SearchContainer<ChangesListItem>> GetPeopleChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPeopleChangesAsync(page, startDate, endDate, cancellationToken);
    }

    public Task<SearchContainer<ChangesListItem>> GetTvChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvChangesAsync(page, startDate, endDate, cancellationToken);
    }

    public Task<IList<Change>> GetMovieChangesAsync(int movieId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieChangesAsync(movieId, page, startDate, endDate, cancellationToken);
    }

    public Task<IList<Change>> GetPersonChangesAsync(int personId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonChangesAsync(personId, page, startDate, endDate, cancellationToken);
    }

    public Task<IList<Change>> GetTvSeasonChangesAsync(int seasonId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonChangesAsync(seasonId, page, startDate, endDate, cancellationToken);
    }

    public Task<IList<Change>> GetTvEpisodeChangesAsync(int episodeId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeChangesAsync(episodeId, page, startDate, endDate, cancellationToken);
    }

    public Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCollectionAsync(collectionId, extraMethods, cancellationToken);
    }

    public Task<Collection> GetCollectionAsync(int collectionId, string language, string includeImageLanguages,
        CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCollectionAsync(collectionId, language, includeImageLanguages, extraMethods, cancellationToken);
    }

    public Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCollectionImagesAsync(collectionId, language, cancellationToken);
    }

    public Task<Company> GetCompanyAsync(int companyId, CompanyMethods extraMethods = CompanyMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCompanyAsync(companyId, extraMethods, cancellationToken);
    }

    public Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCompanyMoviesAsync(companyId, page, cancellationToken);
    }

    public Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCompanyMoviesAsync(companyId, language, page, cancellationToken);
    }

    public Task<APIConfiguration> GetApiConfiguration(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetAPIConfiguration(cancellationToken);
    }

    public Task<List<Country>> GetCountriesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCountriesAsync(cancellationToken);
    }

    public Task<List<Language>> GetLanguagesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetLanguagesAsync(cancellationToken);
    }

    public Task<List<string>> GetPrimaryTranslationsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPrimaryTranslationsAsync(cancellationToken);
    }

    public Task<Timezones> GetTimezonesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTimezonesAsync(cancellationToken);
    }

    public Task<List<Job>> GetJobsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetJobsAsync(cancellationToken);
    }

    public Task<Credit> GetCreditsAsync(string id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCreditsAsync(id, cancellationToken);
    }

    public Task<Credit> GetCreditsAsync(string id, string language, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetCreditsAsync(id, language, cancellationToken);
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

    public DiscoverTv DiscoverTvShowsAsync()
    {
        return _tmDbClient.DiscoverTvShowsAsync();
    }

    public Task<FindContainer> FindAsync(FindExternalSource source, string id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.FindAsync(source, id, cancellationToken);
    }

    public Task<FindContainer> FindAsync(FindExternalSource source, string id, string language,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.FindAsync(source, id, language, cancellationToken);
    }
    
    public Task<List<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieGenresAsync(cancellationToken);
    }

    public Task<List<Genre>> GetMovieGenresAsync(string language, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieGenresAsync(language, cancellationToken);
    }

    public Task<List<Genre>> GetTvGenresAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvGenresAsync(cancellationToken);
    }

    public Task<List<Genre>> GetTvGenresAsync(string language, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvGenresAsync(language, cancellationToken);
    }

    public Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetGuestSessionRatedMoviesAsync(page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetGuestSessionRatedMoviesAsync(language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetGuestSessionRatedTvAsync(page, cancellationToken);
    }

    public Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetGuestSessionRatedTvAsync(language, page, cancellationToken);
    }

    public Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetGuestSessionRatedTvEpisodesAsync(page, cancellationToken);
    }

    public Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetGuestSessionRatedTvEpisodesAsync(language, page, cancellationToken);
    }

    public Task<Keyword> GetKeywordAsync(int keywordId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetKeywordAsync(keywordId, cancellationToken);
    }

    public Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetKeywordMoviesAsync(keywordId, page, cancellationToken);
    }

    public Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetKeywordMoviesAsync(keywordId, language, page, cancellationToken);
    }

    public Task<GenericList> GetListAsync(string listId, string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetListAsync(listId, language, cancellationToken);
    }

    public Task<bool> GetListIsMoviePresentAsync(string listId, int movieId,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetListIsMoviePresentAsync(listId, movieId, cancellationToken);
    }

    public Task<bool> ListAddMovieAsync(string listId, int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.ListAddMovieAsync(listId, movieId, cancellationToken);
    }

    public Task<bool> ListClearAsync(string listId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.ListClearAsync(listId, cancellationToken);
    }

    public Task<string> ListCreateAsync(string name, string description = "", string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.ListCreateAsync(name, description, language, cancellationToken);
    }

    public Task<bool> ListDeleteAsync(string listId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.ListDeleteAsync(listId, cancellationToken);
    }

    public Task<bool> ListRemoveMovieAsync(string listId, int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.ListRemoveMovieAsync(listId, movieId, cancellationToken);
    }

    public Task<AccountState> GetMovieAccountStateAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAccountStateAsync(movieId, cancellationToken);
    }

    public Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAlternativeTitlesAsync(movieId, cancellationToken);
    }

    public Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, string country,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAlternativeTitlesAsync(movieId, country, cancellationToken);
    }

    public Task<Movie> GetMovieAsync(int movieId, MovieMethods extraMethods = MovieMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAsync(movieId, extraMethods, cancellationToken);
    }

    public Task<Movie> GetMovieAsync(string imdbId, MovieMethods extraMethods = MovieMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAsync(imdbId, extraMethods, cancellationToken);
    }

    public Task<Movie> GetMovieAsync(int movieId, string language, string? includeImageLanguage = null,
        MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAsync(movieId, language, includeImageLanguage, extraMethods, cancellationToken);
    }

    public Task<Movie> GetMovieAsync(string imdbId, string language, string? includeImageLanguage = null,
        MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieAsync(imdbId, language, includeImageLanguage, extraMethods, cancellationToken);
    }

    public Task<Credits> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieCreditsAsync(movieId, cancellationToken);
    }

    public Task<ExternalIdsMovie> GetMovieExternalIdsAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieExternalIdsAsync(id, cancellationToken);
    }

    public Task<ImagesWithId> GetMovieImagesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieImagesAsync(movieId, cancellationToken);
    }

    public Task<ImagesWithId> GetMovieImagesAsync(int movieId, string language, string? includeImageLanguage = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieImagesAsync(movieId, language, includeImageLanguage, cancellationToken);
    }

    public Task<KeywordsContainer> GetMovieKeywordsAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieKeywordsAsync(movieId, cancellationToken);
    }

    public Task<Movie> GetMovieLatestAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieLatestAsync(cancellationToken);
    }

    public Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieListsAsync(movieId, page, cancellationToken);
    }

    public Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieListsAsync(movieId, language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieRecommendationsAsync(id, page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieRecommendationsAsync(id, language, page, cancellationToken);
    }

    public Task<SearchContainerWithDates<SearchMovie>> GetMovieNowPlayingListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieNowPlayingListAsync(language, page, region, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMoviePopularListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMoviePopularListAsync(language, page, region, cancellationToken);
    }

    public Task<ResultContainer<ReleaseDatesContainer>> GetMovieReleaseDatesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieReleaseDatesAsync(movieId, cancellationToken);
    }

    public Task<Releases> GetMovieReleasesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieReleasesAsync(movieId, cancellationToken);
    }

    public Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieReviewsAsync(movieId, page, cancellationToken);
    }

    public Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieReviewsAsync(movieId, language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieSimilarAsync(movieId, page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieSimilarAsync(movieId, language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetMovieTopRatedListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieTopRatedListAsync(language, page, region, cancellationToken);
    }

    public Task<TranslationsContainer> GetMovieTranslationsAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieTranslationsAsync(movieId, cancellationToken);
    }

    public Task<SearchContainerWithDates<SearchMovie>> GetMovieUpcomingListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieUpcomingListAsync(language, page, region, cancellationToken);
    }

    public Task<ResultContainer<Video>> GetMovieVideosAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieVideosAsync(movieId, cancellationToken);
    }

    public Task<SingleResultContainer<Dictionary<string, WatchProviders>>> GetMovieWatchProvidersAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieWatchProvidersAsync(movieId, cancellationToken);
    }

    public Task<bool> MovieRemoveRatingAsync(int movieId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.MovieRemoveRatingAsync(movieId, cancellationToken);
    }

    public Task<bool> MovieSetRatingAsync(int movieId, double rating, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.MovieSetRatingAsync(movieId, rating, cancellationToken);
    }

    public Task<Network> GetNetworkAsync(int networkId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetNetworkAsync(networkId, cancellationToken);
    }

    public Task<NetworkLogos> GetNetworkImagesAsync(int networkId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetNetworkImagesAsync(networkId, cancellationToken);
    }

    public Task<AlternativeNames> GetNetworkAlternativeNamesAsync(int networkId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetNetworkAlternativeNamesAsync(networkId, cancellationToken);
    }

    public Task<Person> GetLatestPersonAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetLatestPersonAsync(cancellationToken);
    }

    public Task<Person> GetPersonAsync(int personId, PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonAsync(personId, extraMethods, cancellationToken);
    }

    public Task<Person> GetPersonAsync(int personId, string language, PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonAsync(personId, language, extraMethods, cancellationToken);
    }

    public Task<ExternalIdsPerson> GetPersonExternalIdsAsync(int personId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonExternalIdsAsync(personId, cancellationToken);
    }

    public Task<ProfileImages> GetPersonImagesAsync(int personId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonImagesAsync(personId, cancellationToken);
    }

    public Task<SearchContainer<SearchPerson>> GetPersonPopularListAsync(int page = 0, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonPopularListAsync(page, language, cancellationToken);
    }

    public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonMovieCreditsAsync(personId, cancellationToken);
    }

    public Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, string language,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonMovieCreditsAsync(personId, language, cancellationToken);
    }

    public Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, int page, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonTaggedImagesAsync(personId, page, cancellationToken);
    }

    public Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, string language, int page,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonTaggedImagesAsync(personId, language, page, cancellationToken);
    }

    public Task<TvCredits> GetPersonTvCreditsAsync(int personId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonTvCreditsAsync(personId, cancellationToken);
    }

    public Task<TvCredits> GetPersonTvCreditsAsync(int personId, string language,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetPersonTvCreditsAsync(personId, language, cancellationToken);
    }

    public Task<Review> GetReviewAsync(string reviewId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetReviewAsync(reviewId, cancellationToken);
    }

    public Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchCollectionAsync(query, page, cancellationToken);
    }

    public Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchCollectionAsync(query, language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchCompany>> SearchCompanyAsync(string query, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchCompanyAsync(query, page, cancellationToken);
    }

    public Task<SearchContainer<SearchKeyword>> SearchKeywordAsync(string query, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchKeywordAsync(query, page, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null,
        int primaryReleaseYear = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchMovieAsync(query, page, includeAdult, year, region, primaryReleaseYear, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, string language, int page = 0, bool includeAdult = false, int year = 0,
        string? region = null, int primaryReleaseYear = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchMovieAsync(query, language, page, includeAdult, year, region, primaryReleaseYear, cancellationToken);
    }

    public Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchMultiAsync(query, page, includeAdult, year, region, cancellationToken);
    }

    public Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, string language, int page = 0, bool includeAdult = false, int year = 0,
        string? region = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchMultiAsync(query, language, page, includeAdult, year, region, cancellationToken);
    }

    public Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, int page = 0, bool includeAdult = false, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchPersonAsync(query, page, includeAdult, region, cancellationToken);
    }

    public Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, string language, int page = 0, bool includeAdult = false, string? region = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchPersonAsync(query, language, page, includeAdult, region, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, int page = 0, bool includeAdult = false, int firstAirDateYear = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchTvShowAsync(query, page, includeAdult, firstAirDateYear, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, string language, int page = 0, bool includeAdult = false, int firstAirDateYear = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.SearchTvShowAsync(query, language, page, includeAdult, firstAirDateYear, cancellationToken);
    }

    public Task<SearchContainer<SearchMovie>> GetTrendingMoviesAsync(TimeWindow timeWindow, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTrendingMoviesAsync(timeWindow, page, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTrendingTvAsync(TimeWindow timeWindow, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTrendingTvAsync(timeWindow, page, cancellationToken);
    }

    public Task<SearchContainer<SearchPerson>> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTrendingPeopleAsync(timeWindow, page, cancellationToken);
    }

    public Task<TvGroupCollection> GetTvEpisodeGroupsAsync(string id, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeGroupsAsync(id, language, cancellationToken);
    }

    public Task<TvEpisodeAccountState> GetTvEpisodeAccountStateAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeAccountStateAsync(tvShowId, seasonNumber, episodeNumber, cancellationToken);
    }

    public Task<TvEpisode> GetTvEpisodeAsync(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods extraMethods = TvEpisodeMethods.Undefined,
        string? language = null, string? includeImageLanguage = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeAsync(tvShowId, seasonNumber, episodeNumber, extraMethods, language, includeImageLanguage, cancellationToken);
    }

    public Task<ResultContainer<TvEpisodeInfo>> GetTvEpisodesScreenedTheatricallyAsync(int tvShowId,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodesScreenedTheatricallyAsync(tvShowId, cancellationToken);
    }

    public Task<CreditsWithGuestStars> GetTvEpisodeCreditsAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeCreditsAsync(tvShowId, seasonNumber, episodeNumber, language, cancellationToken);
    }

    public Task<ExternalIdsTvEpisode> GetTvEpisodeExternalIdsAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeExternalIdsAsync(tvShowId, seasonNumber, episodeNumber, cancellationToken);
    }

    public Task<StillImages> GetTvEpisodeImagesAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeImagesAsync(tvShowId, seasonNumber, episodeNumber, language, cancellationToken);
    }

    public Task<ResultContainer<Video>> GetTvEpisodeVideosAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvEpisodeVideosAsync(tvShowId, seasonNumber, episodeNumber, cancellationToken);
    }

    public Task<bool> TvEpisodeRemoveRatingAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.TvEpisodeRemoveRatingAsync(tvShowId, seasonNumber, episodeNumber, cancellationToken);
    }

    public Task<bool> TvEpisodeSetRatingAsync(int tvShowId, int seasonNumber, int episodeNumber, double rating,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.TvEpisodeSetRatingAsync(tvShowId, seasonNumber, episodeNumber, rating, cancellationToken);
    }

    public Task<ResultContainer<TvEpisodeAccountStateWithNumber>> GetTvSeasonAccountStateAsync(int tvShowId, int seasonNumber,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonAccountStateAsync(tvShowId, seasonNumber, cancellationToken);
    }

    public Task<TvSeason> GetTvSeasonAsync(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods = TvSeasonMethods.Undefined, string? language = null,
        string? includeImageLanguage = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonAsync(tvShowId, seasonNumber, extraMethods, language, includeImageLanguage, cancellationToken);
    }

    public Task<TMDbLib.Objects.TvShows.Credits> GetTvSeasonCreditsAsync(int tvShowId, int seasonNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonCreditsAsync(tvShowId, seasonNumber, language, cancellationToken);
    }

    public Task<ExternalIdsTvSeason> GetTvSeasonExternalIdsAsync(int tvShowId, int seasonNumber,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonExternalIdsAsync(tvShowId, seasonNumber, cancellationToken);
    }

    public Task<PosterImages> GetTvSeasonImagesAsync(int tvShowId, int seasonNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonImagesAsync(tvShowId, seasonNumber, language, cancellationToken);
    }

    public Task<ResultContainer<Video>> GetTvSeasonVideosAsync(int tvShowId, int seasonNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvSeasonVideosAsync(tvShowId, seasonNumber, language, cancellationToken);
    }

    public Task<TvShow> GetLatestTvShowAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetLatestTvShowAsync(cancellationToken);
    }

    public Task<AccountState> GetTvShowAccountStateAsync(int tvShowId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowAccountStateAsync(tvShowId, cancellationToken);
    }

    public Task<ResultContainer<AlternativeTitle>> GetTvShowAlternativeTitlesAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowAlternativeTitlesAsync(id, cancellationToken);
    }

    public Task<TvShow> GetTvShowAsync(int id, TvShowMethods extraMethods = TvShowMethods.Undefined, string? language = null,
        string? includeImageLanguage = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowAsync(id, extraMethods, language, includeImageLanguage, cancellationToken);
    }

    public Task<ChangesContainer> GetTvShowChangesAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowChangesAsync(id, cancellationToken);
    }

    public Task<ResultContainer<ContentRating>> GetTvShowContentRatingsAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowContentRatingsAsync(id, cancellationToken);
    }

    public Task<TMDbLib.Objects.TvShows.Credits> GetTvShowCreditsAsync(int id, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowCreditsAsync(id, language, cancellationToken);
    }

    public Task<CreditsAggregate> GetAggregateCredits(int id, string? language = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetAggregateCredits(id, language, cancellationToken);
    }

    public Task<ExternalIdsTvShow> GetTvShowExternalIdsAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowExternalIdsAsync(id, cancellationToken);
    }

    public Task<ImagesWithId> GetTvShowImagesAsync(int id, string? language = null, string? includeImageLanguage = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowImagesAsync(id, language, includeImageLanguage, cancellationToken);
    }

    public Task<SearchContainerWithId<ReviewBase>> GetTvShowReviewsAsync(int id, string? language = null, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowReviewsAsync(id, language, page, cancellationToken);
    }

    public Task<ResultContainer<Keyword>> GetTvShowKeywordsAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowKeywordsAsync(id, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, int page = 0, string? timezone = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowListAsync(list, page, timezone, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, string language, int page = 0, string? timezone = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowListAsync(list, language, page, timezone, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowPopularAsync(int page = -1, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowPopularAsync(page, language, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowSimilarAsync(id, page, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowSimilarAsync(id, language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, int page = 0, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowRecommendationsAsync(id, page, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowRecommendationsAsync(id, language, page, cancellationToken);
    }

    public Task<SearchContainer<SearchTv>> GetTvShowTopRatedAsync(int page = -1, string? language = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowTopRatedAsync(page, language, cancellationToken);
    }

    public Task<TranslationsContainerTv> GetTvShowTranslationsAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowTranslationsAsync(id, cancellationToken);
    }

    public Task<ResultContainer<Video>> GetTvShowVideosAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowVideosAsync(id, cancellationToken);
    }

    public Task<SingleResultContainer<Dictionary<string, WatchProviders>>> GetTvShowWatchProvidersAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvShowWatchProvidersAsync(id, cancellationToken);
    }

    public Task<bool> TvShowRemoveRatingAsync(int tvShowId, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.TvShowRemoveRatingAsync(tvShowId, cancellationToken);
    }

    public Task<bool> TvShowSetRatingAsync(int tvShowId, double rating, CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.TvShowSetRatingAsync(tvShowId, rating, cancellationToken);
    }

    public Task<ResultContainer<WatchProviderRegion>> GetWatchProviderRegionsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetWatchProviderRegionsAsync(cancellationToken);
    }

    public Task<ResultContainer<WatchProviderItem>> GetMovieWatchProvidersAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetMovieWatchProvidersAsync(cancellationToken);
    }

    public Task<ResultContainer<WatchProviderItem>> GetTvWatchProvidersAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _tmDbClient.GetTvWatchProvidersAsync(cancellationToken);
    }
    
}