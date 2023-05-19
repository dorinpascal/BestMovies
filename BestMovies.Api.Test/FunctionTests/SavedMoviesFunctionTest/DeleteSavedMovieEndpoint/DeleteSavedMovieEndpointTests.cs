using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.SavedMoviesFunctionTest.DeleteSavedMovieEndpoint;

public class DeleteSavedMovieEndpointTests
{
    private readonly ISavedMoviesRepository _savedMoviesRepository;
    private readonly SavedMoviesFunctions _sut;
    private readonly MockLogger<SavedMoviesFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public DeleteSavedMovieEndpointTests()
    {
        _savedMoviesRepository = Substitute.For<ISavedMoviesRepository>();
        _logger = Substitute.For<MockLogger<SavedMoviesFunctions>>();
        _sut = new SavedMoviesFunctions(_savedMoviesRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext());
    }

    [Fact]
    public async Task DeleteSavedMovie_SavedMovieRepositoryThrowsException_ReturnsSC500()
    {
        //Arrange
        _savedMoviesRepository.DeleteSavedMovie(Arg.Any<string>(), Arg.Any<int>()).Throws(new Exception());

        //Act
        var response = await _sut.DeleteSavedMovie(_request, "userId",1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task DeleteSavedMovie_SavedMovieRepositoryThrowsArgumentException_ReturnsSC400()
    {
        //Arrange
        string expectedExceptionMessage = "exception";
        _savedMoviesRepository.DeleteSavedMovie(Arg.Any<string>(), Arg.Any<int>()).Throws(new ArgumentException(expectedExceptionMessage));

        //Act
        var response = await _sut.DeleteSavedMovie(_request, "userId",1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }

    [Fact]
    public async Task DeleteSavedMovie_MovieWasDeletedWithSuccess_ReturnsSC200()
    {
        //Arrange
        _savedMoviesRepository.DeleteSavedMovie(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.CompletedTask);

        //Act
        var response = await _sut.DeleteSavedMovie(_request, "userId",1, _logger);
        var result = (OkResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
}
