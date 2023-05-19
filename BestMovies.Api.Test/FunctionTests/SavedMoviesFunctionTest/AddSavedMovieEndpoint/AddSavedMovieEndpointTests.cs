using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.SavedMoviesFunctionTest.AddSavedMovieEndpoint;

public class AddSavedMovieEndpointTests
{
    private readonly ISavedMoviesRepository _savedMoviesRepository;
    private readonly SavedMoviesFunctions _sut;
    private readonly MockLogger<SavedMoviesFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public AddSavedMovieEndpointTests()
    {
        _savedMoviesRepository = Substitute.For<ISavedMoviesRepository>();
        _logger = Substitute.For<MockLogger<SavedMoviesFunctions>>();
        _sut = new SavedMoviesFunctions(_savedMoviesRepository);
        var savedMovie = new SavedMovieDto(1, false);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(savedMovie))),
        };
    }

    [Fact]
    public async Task AddSavedMovie_SavedMovieRepositoryThrowsError_ReturnsSC500()
    {
        //Arrange
        _savedMoviesRepository.SaveMovie(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Throws(new Exception());

        //Act
        var response = await _sut.AddSavedMovie(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task AddSavedMovie_SavedMovieRepositoryThrowsDuplicateException_ReturnsSC409()
    {
        //Arrange
        string expectedExceptionMessage = "exception";
        _savedMoviesRepository.SaveMovie(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Throws(new DuplicateException(expectedExceptionMessage));

        //Act
        var response = await _sut.AddSavedMovie(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(409, result.StatusCode);
        Assert.Equal(expectedExceptionMessage,result.Content);
    }

    [Fact]
    public async Task AddSavedMovie_SavedMovieRepositoryThrowsArgumentException_ReturnsSC400()
    {
        //Arrange
        string expectedExceptionMessage = "exception";
        _savedMoviesRepository.SaveMovie(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Throws(new ArgumentException(expectedExceptionMessage));

        //Act
        var response = await _sut.AddSavedMovie(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }

    [Fact]
    public async Task AddSavedMovie_MovieWasSavedWithSuccess_ReturnsSC200()
    {
        //Arrange
        _savedMoviesRepository.SaveMovie(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(Task.CompletedTask);

        //Act
        var response = await _sut.AddSavedMovie(_request, "userId", _logger);
        var result = (OkResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }

}
