using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;

namespace BestMovies.Api.Test.FunctionTests.SavedMoviesFunctionTest.GetSavedMoviesEndpoint;

public class ParamtersTests
{
    private readonly ISavedMoviesRepository _savedMoviesRepository;
    private readonly SavedMoviesFunctions _sut;
    private readonly MockLogger<SavedMoviesFunctions> _logger;

    public ParamtersTests()
    {
        _savedMoviesRepository = Substitute.For<ISavedMoviesRepository>();
        _logger = Substitute.For<MockLogger<SavedMoviesFunctions>>();
        _sut = new SavedMoviesFunctions(_savedMoviesRepository);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetSavedMoviesForUser_MovieRepositoryThrowsError_ReturnsSC500_UserIdHasInvalidValues_ReturnsBadRequest(string userId)
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(""))),
        };

        //Act
        var response = await _sut.GetSavedMovies(request, userId, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }


}
