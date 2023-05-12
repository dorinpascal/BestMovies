using BestMovies.Bff.Extensions;
using BestMovies.Bff.Services.Impl;
using BestMovies.Bff.Services.Tmdb;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Xunit.Abstractions;

namespace BestMovies.Bff.Test.ServicesTests.MovieServiceTest;

public class GetPopularMoviesTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITMDbWrapperService _tmdbClient;
    private readonly IMovieService _sut;

    public GetPopularMoviesTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _tmdbClient = Substitute.For<ITMDbWrapperService>();
        _sut = new MovieService(_tmdbClient);
    }

    [Fact]
    public async Task GetPopularMovies_CalledWithNoParams_ReturnsTwoMovies()
    {
        //Arrange
        _tmdbClient.GetMovieGenresAsync().Returns(TestGenreList);

        _tmdbClient.GetMoviePopularListAsync().Returns(Task.FromResult(new SearchContainer<SearchMovie>
        {
            Results = new List<SearchMovie>
            {
                TestSearchMovieAction,
                TestSearchMovieHorror
            }
        }));

        //Act
        var result = await _sut.GetPopularMovies();

        //Assert
        await _tmdbClient.Received(1).GetMoviePopularListAsync();
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetPopularMovies_CalledWithGenre_ReturnsDtoWithSearchedGenre()
    {
        //Arrange
        _tmdbClient.GetMovieGenresAsync().Returns(TestGenreList);
        
        _tmdbClient.GetMoviePopularListByGenreAsync(Arg.Any<Genre>(), Arg.Any<string>(),
                Arg.Any<string>())
            .Returns(Task.FromResult(new SearchContainer<SearchMovie>
            {
                Results = new List<SearchMovie>
                {
                    TestSearchMovieHorror
                }
            }));

        //Act
        var result = await _sut.GetPopularMovies("Horror");
        
        // //Assert
        await _tmdbClient.Received(1).GetMoviePopularListByGenreAsync(Arg.Any<Genre>(), Arg.Any<string>(), Arg.Any<string>());
        Assert.Equivalent(TestSearchMovieHorror.ToDto(TestGenreList), result.First());
    }

    private static List<Genre> TestGenreList =>
        new()
        {
            new Genre
            {
                Id = 1,
                Name = "Action"
            },
            new Genre
            {
                Id = 2,
                Name = "Horror"
            }
        };

    private static SearchMovie TestSearchMovieAction =>
        new()
        {
            Id = 1000,
            Title = "TestMovieAction",
            GenreIds = new List<int>
            {
                1
            }
        };

    private static SearchMovie TestSearchMovieHorror =>
        new()
        {
            Id = 1000,
            Title = "TestMovieHorror",
            GenreIds = new List<int>
            {
                2
            }
        };
}