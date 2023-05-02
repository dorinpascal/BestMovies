using BestMovies.Bff.Functions;
using BestMovies.Api.Test.Helpers;
using BestMovies.Bff.Interface;

namespace BestMovies.Api.Test.MoviesFunctionTest.SearchMovieEndpointTest;

public class BodyParametersTest
{
    private readonly IMovieService _tmDbClient;
    private readonly MockLogger<MovieFunctions> _logger;
    private readonly MovieFunctions _sut;
    public BodyParametersTest()
    {
        _tmDbClient = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
        _sut = new MovieFunctions(_tmDbClient);
    }

    [Fact]
    public async Task SearchMovieEndpoint_BodyParameters_IsNotProvided()
    {
        //Arrange
        var _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };


        // ACT
        var response = await _sut.SearchMovie(_request, _logger);
        var result = (BadRequestObjectResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        _logger.Received().Log(LogLevel.Information, Arg.Is<string>(s => s.Contains("Search parameters were not provided")));
    }
}