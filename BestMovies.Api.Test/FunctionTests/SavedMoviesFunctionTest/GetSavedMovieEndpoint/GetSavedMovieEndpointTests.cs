using BestMovies.Api.Functions;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using Microsoft.Extensions.Primitives;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.SavedMoviesFunctionTest.GetSavedMovieEndpoint;

public class GetSavedMovieEndpointTests
{
    private readonly ISavedMoviesRepository _savedMoviesRepository;
    private readonly SavedMoviesFunctions _sut;
    private readonly MockLogger<SavedMoviesFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public GetSavedMovieEndpointTests()
    {
        _savedMoviesRepository = Substitute.For<ISavedMoviesRepository>();
        _logger = Substitute.For<MockLogger<SavedMoviesFunctions>>();
        _sut = new SavedMoviesFunctions(_savedMoviesRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext());
    }

    [Fact]
    public async Task GetSavedMovieForUser_SavedMovieRepositoryReturnsNull_ReturnsSC404()
    {

        //Arrange
        _savedMoviesRepository.GetSavedMovieForUser(Arg.Any<string>(), Arg.Any<int>()).ReturnsForAnyArgs((SavedMovie?)null);

        //Act
        var response = await _sut.GetSavedMovie(_request, "userId", 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);
    }


    [Fact]
    public async Task GetSavedMovieForUser_SavedMovieRepositoryThrowsArgumentException_ReturnsSC400()
    {

        //Arrange
        _savedMoviesRepository.GetSavedMovieForUser(Arg.Any<string>(), Arg.Any<int>()).Throws(new ArgumentException());

        //Act
        var response = await _sut.GetSavedMovie(_request, "userId", 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }



    [Fact]
    public async Task GetSavedMovieForUser_SavedMovieRepositoryThrowsError_ReturnsSC500()
    {
        //Arrange
        _savedMoviesRepository.GetSavedMovieForUser(Arg.Any<string>(), Arg.Any<int>()).Throws(new Exception());

        //Act
        var response = await _sut.GetSavedMovie(_request, "userId",1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task GetSavedMovieForUser_SavedMovieIsReturned_ReturnsSC200()
    {
        //Arrange
        _savedMoviesRepository.GetSavedMovieForUser(Arg.Any<string>(), Arg.Any<int>()).Returns(new SavedMovie("userId", 1, true));

        //Act
        var response = await _sut.GetSavedMovie(_request, "userId", 1, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
}
