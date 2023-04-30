using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;
using BestMovies.Bff.Functions;
using BestMovies.Api.Test.Helpers;
using BestMovies.Bff.Interface;

namespace BestMovies.Api.Test.MoviesFunctionTest.SearchMovieEndpointTest;

public class SearchMovieEndpointTests
{

    private readonly DefaultHttpRequest request;
    private readonly ITmdbApiWrapper _tmDbClient;
    private readonly MockLogger<MovieFunctions> logger;
    public SearchMovieEndpointTests()
    {
        SearchParametersDto searchParams = new SearchParametersDto("movieTitle");
        request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(searchParams)))
        };
        _tmDbClient = Substitute.For<ITmdbApiWrapper>();
        logger = Substitute.For<MockLogger<MovieFunctions>>();
    }

    [Fact]
    public async Task SearchMovieEndpoint_TmdbApi_NotAvailable()
    {
        //Arrange
        _tmDbClient.SearchMovie(Arg.Any<string>()).Throws(new Exception());

        MovieFunctions function = new(_tmDbClient);

        // ACT
        IActionResult? response = await function.SearchMovie(request, logger);
        ContentResult result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);

    }


    [Fact]
    public async Task SearchMovieEndpoint_ReeturnsListOfMovies_OkObjectResult()
    {
        //Arrange
        IEnumerable<SearchMovieDto> movies = new List<SearchMovieDto>()
        {
               new SearchMovieDto(1,"title", new List<string>()
               {
                   "genre"
               }),
        };

        _tmDbClient.SearchMovie(Arg.Any<string>()).Returns(movies);

        MovieFunctions function = new(_tmDbClient);

        // ACT
        IActionResult? response = await function.SearchMovie(request, logger);
        OkObjectResult result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
        logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Successfully retrieved list of searched movies")));
    }
}