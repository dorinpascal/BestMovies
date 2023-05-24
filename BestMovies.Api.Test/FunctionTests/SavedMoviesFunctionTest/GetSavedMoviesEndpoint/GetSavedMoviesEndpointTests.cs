using BestMovies.Api.Functions;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using Microsoft.Extensions.Primitives;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.SavedMoviesFunctionTest.GetSavedMoviesEndpoint;

public class GetSavedMoviesEndpointTests
{
    private readonly ISavedMoviesRepository _savedMoviesRepository;
    private readonly SavedMoviesFunctions _sut;
    private readonly MockLogger<SavedMoviesFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public GetSavedMoviesEndpointTests()
    {
        _savedMoviesRepository = Substitute.For<ISavedMoviesRepository>();
        _logger = Substitute.For<MockLogger<SavedMoviesFunctions>>();
        _sut = new SavedMoviesFunctions(_savedMoviesRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "userId", "user_id" }
                })
        };
    }

    [Fact]
    public async Task GetSavedMoviesForUser_SavedMovieRepositoryThrowsError_ReturnsSC500()
    {
        //Arrange
        _savedMoviesRepository.GetSavedMoviesForUser(Arg.Any<string>()).Throws(new Exception());

        //Act
        var response = await _sut.GetSavedMovies(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task GetSavedMoviesForUser_SavedMovieRepositoryThrowsArgumentException_ReturnsSC400()
    {
        //Arrange
        _savedMoviesRepository.GetSavedMoviesForUser(Arg.Any<string>()).Throws(new ArgumentException());

        //Act
        var response = await _sut.GetSavedMovies(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }


    [Fact]
    public async Task GetSavedMoviesForUser_ListOfSavedMovieIsReturned_ReturnsSC200()
    {
        //Arrange
        var savedMovies = new List<SavedMovie>() { new SavedMovie("userId",1,true)};
        _savedMoviesRepository.GetSavedMoviesForUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(savedMovies);

        //Act
        var response = await _sut.GetSavedMovies(_request, "userId", _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }


}
