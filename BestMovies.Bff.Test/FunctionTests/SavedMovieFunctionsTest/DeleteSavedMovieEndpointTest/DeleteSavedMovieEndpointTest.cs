using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.SavedMovieFunctionsTest.DeleteSavedMovieEndpointTest;

public class DeleteSavedMovieEndpointTest
{
    private readonly DefaultHttpRequest _request;
    private readonly ISavedMovieService _savedMovieService;
    private readonly MockLogger<SavedMovieFunctions> _logger;
    private readonly SavedMovieFunctions _sut;
    private const string ValidHeader =
        "ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9";

    public DeleteSavedMovieEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _request.Headers.Add("x-ms-client-principal", ValidHeader);
        _savedMovieService = Substitute.For<ISavedMovieService>();
        _logger = Substitute.For<MockLogger<SavedMovieFunctions>>();
        var userService = Substitute.For<IUserService>();
        _sut = new SavedMovieFunctions(userService, _savedMovieService);
    }

    [Fact]
    public async Task DeleteSavedMovieEndpoint_BestMoviesApiNotAvailable_ReturnsSC500()
    {
        //Arrange
        _savedMovieService.DeleteMovie(Arg.Any<int>(), Arg.Any<string>()).Throws(new Exception());

        //Act
        var response = await _sut.DeleteSavedMovie(_request, 0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteSavedMovieEndpoint_UserIsUnauthorized_UnauthorizedResult()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");
        
        //Act
        var response = await _sut.DeleteSavedMovie(_request, 0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteSavedMovieEndpoint_ArgumentException_ReturnsSC400()
    {
        //Arrange
        _savedMovieService.DeleteMovie(Arg.Any<int>(), Arg.Any<string>()).Throws(new ArgumentException("Bad request"));
        
        //Act
        var response = await _sut.DeleteSavedMovie(_request, 0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteSavedMovieEndpoint_ValidRequest_ReturnsSC200()
    {
        //Arrange
        
        //Act
        var response = await _sut.DeleteSavedMovie(_request, 0, _logger);
        var result = (StatusCodeResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
   
}