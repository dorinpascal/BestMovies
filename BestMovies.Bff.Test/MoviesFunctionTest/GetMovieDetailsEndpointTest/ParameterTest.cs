using BestMovies.Bff.Functions;
using BestMovies.Bff.Interface;
using BestMovies.Bff.Test.Helpers;

namespace BestMovies.Bff.Test.MoviesFunctionTest.GetMovieDetailsEndpointTest;

public class ParameterTest
{
    private readonly IMovieService _movieService;
    private readonly MockLogger<MovieFunctions> _logger;

    public ParameterTest()
    {
        _movieService = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
    }


    [Fact]
    public async Task GetMovieDetails_PassedParameter_IsNotValid()
    {
        //Arrange
       

        var function = new MovieFunctions(_movieService);
        var _request = new DefaultHttpRequest(new DefaultHttpContext());
        // ACT
        var response = await function.GetMovieDetails(_request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        _logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Invalid value for the id. The value must be greater than 0")));
    }

}
