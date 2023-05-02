using BestMovies.Api.Test.Helpers;
using BestMovies.Bff.Functions;
using BestMovies.Bff.Interface;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.MoviesFunctionTest.GetPopularMoviesEndpointTest;

public class GetPopularMoviesEndpointTests
{
    private readonly DefaultHttpRequest _request;
    private readonly IMovieService _tmDbClient;
    private readonly MockLogger<MovieFunctions> _logger;
    
    public GetPopularMoviesEndpointTests()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _tmDbClient = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
    }
    
    [Fact]
    public async Task GetPopularMoviesEndpoint_TmdbApi_NotAvailable()
    {
        //Arrange
        _tmDbClient.GetPopularMovies().Throws(new Exception());

        var function = new MovieFunctions(_tmDbClient);

        // ACT
        var response = await function.GetPopularMovies(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);

    }
    
    [Fact]
    public async Task GetPopularMoviesEndpoint_NotFound()
    {
        //Arrange
        _tmDbClient.GetPopularMovies().Throws(new NotFoundException());

        var function = new MovieFunctions(_tmDbClient);

        // ACT
        var response = await function.GetPopularMovies(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);

    }
    
    [Fact]
    public async Task GetPopularMoviesEndpoint_NoArgs_OkObjectResult()
    {
        //Arrange
        var movies = new List<SearchMovieDto>()
        {
            new(1,"title", new List<string>()
            {
                "genre"
            }),
        };

        _tmDbClient.GetPopularMovies().Returns(movies);

        var function = new MovieFunctions(_tmDbClient);

        // ACT
        var response = await function.GetPopularMovies(_request, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
    
}