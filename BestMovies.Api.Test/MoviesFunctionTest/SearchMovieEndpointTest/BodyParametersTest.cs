using BestMovies.Bff.Functions;
using BestMovies.Api.Test.Helpers;
using BestMovies.Bff.Interface;

namespace BestMovies.Api.Test.MoviesFunctionTest.SearchMovieEndpointTest;

public class BodyParametersTest
{

    private readonly DefaultHttpRequest request;
    private readonly ITmdbApiWrapper _tmDbClient;
    private readonly MockLogger<MovieFunctions> logger;
    public BodyParametersTest()
    {

        request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };
        _tmDbClient = Substitute.For<ITmdbApiWrapper>();
        logger = Substitute.For<MockLogger<MovieFunctions>>();
    }

    [Fact]
    public async Task SearchMovieEndpoint_BodyParameters_IsNotProvided()
    {
        //Arrange
        MovieFunctions function = new(_tmDbClient);

        // ACT
        IActionResult? response = await function.SearchMovie(request, logger);
        BadRequestObjectResult result = (BadRequestObjectResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Search paramteres were not provided")));
    }
}