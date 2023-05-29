using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.StatisticsFunctionsTest;

public class GetMovieStatsTest
{
    private readonly DefaultHttpRequest _request;
    private readonly IStatisticsService _statisticsService;
    private readonly IMovieService _movieService;
    private readonly MockLogger<StatisticsFunctions> _logger;
    private readonly StatisticsFunctions _sut;
    
    public GetMovieStatsTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _statisticsService = Substitute.For<IStatisticsService>();
        _movieService = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<StatisticsFunctions>>();
        _sut = new StatisticsFunctions(_statisticsService, _movieService );
    }
    
    [Fact]
    public async Task GetMovieStatsEndpoint_BestMoviesApi_NotAvailable()
    {
        //Arrange
        _statisticsService.GetMovieStats(1).Throws(new Exception());
        
        //Act
        var response = await _sut.GetMovieStats(_request,1,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task GetMovieStatsEndpoint_InvalidValueForId_BadRequestResult()
    {
        //Arrange
        
        
        //Act
        var response = await _sut.GetMovieStats(_request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetMovieStatsEndpoint_MovieStatsFound_ReturnsObjectOkResult()
    {
        //Arrange
        var movieStats = new MovieStatsDto(1, 2, 3, 4);
        _statisticsService.GetMovieStats(1).Returns(movieStats);
        
        //Act
        var response = await _sut.GetMovieStats(_request,1,_logger);
        var result = (ObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(movieStats, result.Value);
    }
}