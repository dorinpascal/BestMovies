using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;
using BestMovies.Bff.Functions;
using BestMovies.Bff.Interface;
using BestMovies.Bff.Test.Helpers;

namespace BestMovies.Bff.Test.MoviesFunctionTest.SearchMovieEndpointTest;

public class SearchMovieEndpointTests
{

    private readonly DefaultHttpRequest _request;
    private readonly IMovieService _movieService;
    private readonly MockLogger<MovieFunctions> _logger;
    private readonly MovieFunctions _sut;
    public SearchMovieEndpointTests()
    {
        SearchParametersDto searchParams = new SearchParametersDto("movieTitle");
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(searchParams)))
        };
        _movieService = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
        _sut = new MovieFunctions(_movieService);
    }

    [Fact]
    public async Task SearchMovieEndpoint_TmdbApi_NotAvailable()
    {
        //Arrange
        _movieService.SearchMovie(Arg.Any<string>()).Throws(new Exception());
        
        // ACT
        var response = await _sut.SearchMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);

    }


    [Fact]
    public async Task SearchMovieEndpoint_ReturnsListOfMovies_OkObjectResult()
    {
        //Arrange
        var movies = new List<SearchMovieDto>()
        {
               new SearchMovieDto(1,"title", new List<string>()
               {
                   "genre"
               }),
        };

        _movieService.SearchMovie(Arg.Any<string>()).Returns(movies);
        
        // ACT
        var response = await _sut.SearchMovie(_request, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
        _logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Successfully retrieved list of searched movies")));
    }
}