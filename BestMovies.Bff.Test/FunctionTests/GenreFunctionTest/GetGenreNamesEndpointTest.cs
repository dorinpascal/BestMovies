using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.GenreFunctionTest;

public class GetGenreNamesEndpointTest
{
    private readonly DefaultHttpRequest _request;
    private readonly IGenreService _genreService;
    private readonly MockLogger<GenreFunctions> _logger;
    private readonly GenreFunctions _sut;

    public GetGenreNamesEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _genreService = Substitute.For<IGenreService>();
        _logger = Substitute.For<MockLogger<GenreFunctions>>();
        _sut = new GenreFunctions(_genreService);
    }
    
    [Fact]
    public async Task GetGenreNames_TmdbApi_NotAvailable()
    {
        //Arrange
        _genreService.GetGenreNames().Throws(new Exception());
        
        //Act
        var response = await _sut.GetGenreNames(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task GetGenreNames_ReturnsList_OkObjectResult()
    {
        //Arrange
        var genres = new List<string>()
        {
            "Action", "Comedy", "Horror"
        };

        _genreService.GetGenreNames().Returns(genres);
        
        //Act
        var response = await _sut.GetGenreNames(_request, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
}