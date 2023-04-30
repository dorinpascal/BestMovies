using BestMovies.Bff.Functions;
using BestMovies.Bff.Interface;
using BestMovies.Bff.Test.Helpers;

namespace BestMovies.Bff.Test.MoviesFunctionTest.SearchMovieEndpointTest;

public class BodyParametersTest
{
    private readonly IMovieService _tmDbClient;
    private readonly MockLogger<MovieFunctions> _logger;
    public BodyParametersTest()
    {
        _tmDbClient = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
    }

    [Fact]
    public async Task SearchMovieEndpoint_BodyParameters_IsNotProvided()
    {
        //Arrange
        var _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };

        var function = new MovieFunctions(_tmDbClient);

        // ACT
        var response = await function.SearchMovie(_request, _logger);
        var result = (BadRequestObjectResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        _logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Search paramteres were not provided")));
    }
}