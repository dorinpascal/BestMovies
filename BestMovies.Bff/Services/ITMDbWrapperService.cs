using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

namespace BestMovies.Bff.Services;

// ReSharper disable once InconsistentNaming
public interface ITMDbWrapperService
{
    Task<TMDbConfig> GetConfigAsync();
    Uri GetImageUrl(string size, string filePath, bool useSsl = false);

    Task<byte[]> GetImageBytesAsync(string size, string filePath, bool useSsl = false,
        CancellationToken token = new CancellationToken());

    void SetConfig(TMDbConfig config);
    Task SetSessionInformationAsync(string sessionId, SessionType sessionType);

    Task<bool> AccountChangeFavoriteStatusAsync(MediaType mediaType, int mediaId, bool isFavorite,
        CancellationToken cancellationToken = new CancellationToken());

    Task<bool> AccountChangeWatchlistStatusAsync(MediaType mediaType, int mediaId, bool isOnWatchlist,
        CancellationToken cancellationToken = new CancellationToken());

    Task<AccountDetails> AccountGetDetailsAsync(CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> AccountGetFavoriteMoviesAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> AccountGetFavoriteTvAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<AccountList>> AccountGetListsAsync(int page = 1, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> AccountGetMovieWatchlistAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovieWithRating>> AccountGetRatedMoviesAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<AccountSearchTvEpisode>> AccountGetRatedTvShowEpisodesAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<AccountSearchTv>> AccountGetRatedTvShowsAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> AccountGetTvWatchlistAsync(int page = 1, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined,
        string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<GuestSession> AuthenticationCreateGuestSessionAsync(CancellationToken cancellationToken = new CancellationToken());

    Task<UserSession> AuthenticationGetUserSessionAsync(string initialRequestToken,
        CancellationToken cancellationToken = new CancellationToken());

    Task<UserSession> AuthenticationGetUserSessionAsync(string username, string password,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Token> AuthenticationRequestAuthenticationTokenAsync(CancellationToken cancellationToken = new CancellationToken());

    Task AuthenticationValidateUserTokenAsync(string initialRequestToken, string username, string password,
        CancellationToken cancellationToken = new CancellationToken());

    Task<CertificationsContainer> GetMovieCertificationsAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<CertificationsContainer> GetTvCertificationsAsync(CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<ChangesListItem>> GetMoviesChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<ChangesListItem>> GetPeopleChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<ChangesListItem>> GetTvChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<IList<Change>> GetMovieChangesAsync(int movieId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<IList<Change>> GetPersonChangesAsync(int personId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<IList<Change>> GetTvSeasonChangesAsync(int seasonId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<IList<Change>> GetTvEpisodeChangesAsync(int episodeId, int page = 0, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Collection> GetCollectionAsync(int collectionId, string language, string includeImageLanguages,
        CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = new CancellationToken());

    Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Company> GetCompanyAsync(int companyId, CompanyMethods extraMethods = CompanyMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<APIConfiguration> GetApiConfiguration(CancellationToken cancellationToken = new CancellationToken());
    Task<List<Country>> GetCountriesAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<List<Language>> GetLanguagesAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<List<string>> GetPrimaryTranslationsAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<Timezones> GetTimezonesAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<List<Job>> GetJobsAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<Credit> GetCreditsAsync(string id, CancellationToken cancellationToken = new CancellationToken());
    Task<Credit> GetCreditsAsync(string id, string language, CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainer<SearchMovie>> GetMoviePopularListByGenreAsync(Genre searchedGenre, string? region, string? language);
    DiscoverMovie DiscoverMoviesAsync();
    DiscoverTv DiscoverTvShowsAsync();
    Task<FindContainer> FindAsync(FindExternalSource source, string id, CancellationToken cancellationToken = new CancellationToken());

    Task<FindContainer> FindAsync(FindExternalSource source, string id, string language,
        CancellationToken cancellationToken = new CancellationToken());

    Task<List<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<List<Genre>> GetMovieGenresAsync(string language, CancellationToken cancellationToken = new CancellationToken());
    Task<List<Genre>> GetTvGenresAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<List<Genre>> GetTvGenresAsync(string language, CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovieWithRating>> GetGuestSessionRatedMoviesAsync(string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTvShowWithRating>> GetGuestSessionRatedTvAsync(string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodesAsync(string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Keyword> GetKeywordAsync(int keywordId, CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<SearchMovie>> GetKeywordMoviesAsync(int keywordId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<GenericList> GetListAsync(string listId, string? language = null, CancellationToken cancellationToken = new CancellationToken());

    Task<bool> GetListIsMoviePresentAsync(string listId, int movieId,
        CancellationToken cancellationToken = new CancellationToken());

    Task<bool> ListAddMovieAsync(string listId, int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<bool> ListClearAsync(string listId, CancellationToken cancellationToken = new CancellationToken());

    Task<string> ListCreateAsync(string name, string description = "", string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<bool> ListDeleteAsync(string listId, CancellationToken cancellationToken = new CancellationToken());
    Task<bool> ListRemoveMovieAsync(string listId, int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<AccountState> GetMovieAccountStateAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());

    Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, string country,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Movie> GetMovieAsync(int movieId, MovieMethods extraMethods = MovieMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Movie> GetMovieAsync(string imdbId, MovieMethods extraMethods = MovieMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Movie> GetMovieAsync(int movieId, string language, string? includeImageLanguage = null,
        MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = new CancellationToken());

    Task<Movie> GetMovieAsync(string imdbId, string language, string? includeImageLanguage = null,
        MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = new CancellationToken());

    Task<Credits> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<ExternalIdsMovie> GetMovieExternalIdsAsync(int id, CancellationToken cancellationToken = new CancellationToken());
    Task<ImagesWithId> GetMovieImagesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());

    Task<ImagesWithId> GetMovieImagesAsync(int movieId, string language, string? includeImageLanguage = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<KeywordsContainer> GetMovieKeywordsAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<Movie> GetMovieLatestAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithDates<SearchMovie>> GetMovieNowPlayingListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetMoviePopularListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<ReleaseDatesContainer>> GetMovieReleaseDatesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<Releases> GetMovieReleasesAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetMovieTopRatedListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TranslationsContainer> GetMovieTranslationsAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithDates<SearchMovie>> GetMovieUpcomingListAsync(string? language = null, int page = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<Video>> GetMovieVideosAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<SingleResultContainer<Dictionary<string, WatchProviders>>> GetMovieWatchProvidersAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<ResultContainer<WatchProviderItem>> GetMovieWatchProvidersAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<bool> MovieRemoveRatingAsync(int movieId, CancellationToken cancellationToken = new CancellationToken());
    Task<bool> MovieSetRatingAsync(int movieId, double rating, CancellationToken cancellationToken = new CancellationToken());
    Task<Network> GetNetworkAsync(int networkId, CancellationToken cancellationToken = new CancellationToken());
    Task<NetworkLogos> GetNetworkImagesAsync(int networkId, CancellationToken cancellationToken = new CancellationToken());
    Task<AlternativeNames> GetNetworkAlternativeNamesAsync(int networkId, CancellationToken cancellationToken = new CancellationToken());
    Task<Person> GetLatestPersonAsync(CancellationToken cancellationToken = new CancellationToken());

    Task<Person> GetPersonAsync(int personId, PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Person> GetPersonAsync(int personId, string language, PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ExternalIdsPerson> GetPersonExternalIdsAsync(int personId, CancellationToken cancellationToken = new CancellationToken());
    Task<ProfileImages> GetPersonImagesAsync(int personId, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchPerson>> GetPersonPopularListAsync(int page = 0, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, CancellationToken cancellationToken = new CancellationToken());

    Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, string language,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, int page, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, string language, int page,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TvCredits> GetPersonTvCreditsAsync(int personId, CancellationToken cancellationToken = new CancellationToken());

    Task<TvCredits> GetPersonTvCreditsAsync(int personId, string language,
        CancellationToken cancellationToken = new CancellationToken());

    Task<Review> GetReviewAsync(string reviewId, CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchCompany>> SearchCompanyAsync(string query, int page = 0, CancellationToken cancellationToken = new CancellationToken());
    Task<SearchContainer<SearchKeyword>> SearchKeywordAsync(string query, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null,
        int primaryReleaseYear = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, string language, int page = 0, bool includeAdult = false, int year = 0,
        string? region = null, int primaryReleaseYear = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, int page = 0, bool includeAdult = false, int year = 0, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, string language, int page = 0, bool includeAdult = false, int year = 0,
        string? region = null, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, int page = 0, bool includeAdult = false, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, string language, int page = 0, bool includeAdult = false, string? region = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, int page = 0, bool includeAdult = false, int firstAirDateYear = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, string language, int page = 0, bool includeAdult = false, int firstAirDateYear = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchMovie>> GetTrendingMoviesAsync(TimeWindow timeWindow, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTrendingTvAsync(TimeWindow timeWindow, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchPerson>> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TvGroupCollection> GetTvEpisodeGroupsAsync(string id, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TvEpisodeAccountState> GetTvEpisodeAccountStateAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TvEpisode> GetTvEpisodeAsync(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods extraMethods = TvEpisodeMethods.Undefined,
        string? language = null, string? includeImageLanguage = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<TvEpisodeInfo>> GetTvEpisodesScreenedTheatricallyAsync(int tvShowId,
        CancellationToken cancellationToken = new CancellationToken());

    Task<CreditsWithGuestStars> GetTvEpisodeCreditsAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ExternalIdsTvEpisode> GetTvEpisodeExternalIdsAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken());

    Task<StillImages> GetTvEpisodeImagesAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<Video>> GetTvEpisodeVideosAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken());

    Task<bool> TvEpisodeRemoveRatingAsync(int tvShowId, int seasonNumber, int episodeNumber,
        CancellationToken cancellationToken = new CancellationToken());

    Task<bool> TvEpisodeSetRatingAsync(int tvShowId, int seasonNumber, int episodeNumber, double rating,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<TvEpisodeAccountStateWithNumber>> GetTvSeasonAccountStateAsync(int tvShowId, int seasonNumber,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TvSeason> GetTvSeasonAsync(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods = TvSeasonMethods.Undefined, string? language = null,
        string? includeImageLanguage = null, CancellationToken cancellationToken = new CancellationToken());

    Task<TMDbLib.Objects.TvShows.Credits> GetTvSeasonCreditsAsync(int tvShowId, int seasonNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ExternalIdsTvSeason> GetTvSeasonExternalIdsAsync(int tvShowId, int seasonNumber,
        CancellationToken cancellationToken = new CancellationToken());

    Task<PosterImages> GetTvSeasonImagesAsync(int tvShowId, int seasonNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<Video>> GetTvSeasonVideosAsync(int tvShowId, int seasonNumber, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TvShow> GetLatestTvShowAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<AccountState> GetTvShowAccountStateAsync(int tvShowId, CancellationToken cancellationToken = new CancellationToken());
    Task<ResultContainer<AlternativeTitle>> GetTvShowAlternativeTitlesAsync(int id, CancellationToken cancellationToken = new CancellationToken());

    Task<TvShow> GetTvShowAsync(int id, TvShowMethods extraMethods = TvShowMethods.Undefined, string? language = null,
        string? includeImageLanguage = null, CancellationToken cancellationToken = new CancellationToken());

    Task<ChangesContainer> GetTvShowChangesAsync(int id, CancellationToken cancellationToken = new CancellationToken());
    Task<ResultContainer<ContentRating>> GetTvShowContentRatingsAsync(int id, CancellationToken cancellationToken = new CancellationToken());

    Task<TMDbLib.Objects.TvShows.Credits> GetTvShowCreditsAsync(int id, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<CreditsAggregate> GetAggregateCredits(int id, string? language = null, CancellationToken cancellationToken = new CancellationToken());
    Task<ExternalIdsTvShow> GetTvShowExternalIdsAsync(int id, CancellationToken cancellationToken = new CancellationToken());

    Task<ImagesWithId> GetTvShowImagesAsync(int id, string? language = null, string? includeImageLanguage = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainerWithId<ReviewBase>> GetTvShowReviewsAsync(int id, string? language = null, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<ResultContainer<Keyword>> GetTvShowKeywordsAsync(int id, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, int page = 0, string? timezone = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, string language, int page = 0, string? timezone = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowPopularAsync(int page = -1, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, int page = 0, CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id, string language, int page = 0,
        CancellationToken cancellationToken = new CancellationToken());

    Task<SearchContainer<SearchTv>> GetTvShowTopRatedAsync(int page = -1, string? language = null,
        CancellationToken cancellationToken = new CancellationToken());

    Task<TranslationsContainerTv> GetTvShowTranslationsAsync(int id, CancellationToken cancellationToken = new CancellationToken());
    Task<ResultContainer<Video>> GetTvShowVideosAsync(int id, CancellationToken cancellationToken = new CancellationToken());
    Task<SingleResultContainer<Dictionary<string, WatchProviders>>> GetTvShowWatchProvidersAsync(int id, CancellationToken cancellationToken = new CancellationToken());
    Task<bool> TvShowRemoveRatingAsync(int tvShowId, CancellationToken cancellationToken = new CancellationToken());
    Task<bool> TvShowSetRatingAsync(int tvShowId, double rating, CancellationToken cancellationToken = new CancellationToken());
    Task<ResultContainer<WatchProviderRegion>> GetWatchProviderRegionsAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<ResultContainer<WatchProviderItem>> GetTvWatchProvidersAsync(CancellationToken cancellationToken = new CancellationToken());
}