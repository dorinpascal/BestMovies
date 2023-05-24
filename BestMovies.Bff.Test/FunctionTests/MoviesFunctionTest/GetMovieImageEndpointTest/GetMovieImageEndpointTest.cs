using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.MoviesFunctionTest.GetMovieImageEndpointTest;

public class GetMovieImageEndpointTest
{
    private readonly DefaultHttpRequest _request;
    private readonly IMovieService _movieService;
    private readonly MockLogger<MovieFunctions> _logger;
    private readonly MovieFunctions _sut;
    
    public GetMovieImageEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _movieService = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
        _sut = new MovieFunctions(_movieService);
    }
    
    [Fact]
    public async Task GetMovieImageEndpoint_TmdbApi_NotAvailable()
    {
        //Arrange
        _movieService.GetImageBytes(Arg.Any<string>(), Arg.Any<int>()).Throws(new Exception());
        
        //Act
        var response = await _sut.GetMovieImage(_request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task GetMovieImageEndpoint_ImageNotFound_NotFoundException()
    {
        //Arrange
        _movieService.GetImageBytes(Arg.Any<string>(), Arg.Any<int>()).Throws(new NotFoundException("Not found"));
        
        //Act
        var response = await _sut.GetMovieImage(_request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);
    }
    
    [Fact]
    public async Task GetMovieImageEndpoint_InvalidSize_ArgumentException()
    {
        //Arrange
        _movieService.GetImageBytes(Arg.Any<string>(), Arg.Any<int>()).Throws(new ArgumentException("Bad request"));
        
        //Act
        var response = await _sut.GetMovieImage(_request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetMovieImageEndpoint_MovieImageAvailable_ReturnsFileContentResult()
    {
        //Arrange
        var movieImage = new byte[]{0,1,2,3,4,5};
        _movieService.GetImageBytes(Arg.Any<string>(), Arg.Any<int>()).Returns(movieImage);
        
        //Act
        var response = await _sut.GetMovieImage(_request,0,_logger);
        var result = (FileContentResult)response;

        //Assert
        Assert.Equal(movieImage, result.FileContents);
    }
}