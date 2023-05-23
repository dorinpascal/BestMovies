using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Actor;
using BestMovies.Shared.Dtos.Director;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.MoviesFunctionTest.GetMovieDetailsEndpointTest;

public class GetMovieDetailsEndpointTest
{

    private readonly IMovieService _movieService;
    private readonly MockLogger<MovieFunctions> _logger;
    private readonly DefaultHttpRequest _request;
    private readonly MovieFunctions _sut;

    public GetMovieDetailsEndpointTest()
    {
        _movieService = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _sut = new MovieFunctions(_movieService);
    }


    [Fact]
    public async Task GetMovieDetails_TmdbApiNotAvailable_ReturnsStatusCode500()
    {
        //Arrange
        _movieService.GetMovieDetails(Arg.Any<int>()).Throws<Exception>();
        
        //Act
        var response = await _sut.GetMovieDetails(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task GetMovieDetails_NoDetailsAvailable_ReturnsStatusCode404()
    {
        //Arrange
        _movieService.GetMovieDetails(Arg.Any<int>()).Throws(new NotFoundException("Not found"));

        //Act
        var response = await _sut.GetMovieDetails(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);

    }

    [Fact]
    public async Task GetMovieDetails_MovieDetails_ReturnsOkObjectResult()
    {
        //Arrange
        var movieDetailsDto = new MovieDetailsDto(0, "title", "description", "originalLanguage", DateOnly.MinValue, 0, new List<string>(), new List<ActorDto>(), new DirectorDto(1, "Name"));
        _movieService.GetMovieDetails(Arg.Any<int>()).Returns(movieDetailsDto);

        //Act
        var response = await _sut.GetMovieDetails(_request, 1, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);

    }
}
