using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;

namespace BestMovies.Bff.Test.FunctionTests.MoviesFunctionTest.GetMovieDetailsEndpointTest;

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
        var request = new DefaultHttpRequest(new DefaultHttpContext());
        
        //Act
        var response = await function.GetMovieDetails(request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}
