using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.SavedMovieFunctionsTest.GetSavedMovieEndpointTest;

public class GetSavedMovieEndpointTest
{
    private readonly DefaultHttpRequest _request;
    private readonly ISavedMovieService _savedMovieService;
    private readonly MockLogger<SavedMovieFunctions> _logger;
    private readonly SavedMovieFunctions _sut;
    private const string ValidHeader =
        "ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9";
    
    public GetSavedMovieEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _request.Headers.Add("x-ms-client-principal", ValidHeader);
        _savedMovieService = Substitute.For<ISavedMovieService>();
        _logger = Substitute.For<MockLogger<SavedMovieFunctions>>();
        var userService = Substitute.For<IUserService>();
        _sut = new SavedMovieFunctions(userService, _savedMovieService);
    }
    
    [Fact]
    public async Task GetSavedMovieEndpoint_BestMoviesApiNotAvailable_ReturnsSC500()
    {
        //Arrange
        _savedMovieService.GetSavedMovieOrDefault(Arg.Any<int>(), Arg.Any<string>()).Throws(new Exception());
        
        //Act
        var response = await _sut.GetSavedMovie(_request,0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMovieEndpoint_UserIsUnauthorized_UnauthorizedResult()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");
        
        //Act
        var response = await _sut.GetSavedMovie(_request,0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMovieEndpoint_ArgumentException_ReturnsSC400()
    {
        //Arrange
        _savedMovieService.GetSavedMovieOrDefault(Arg.Any<int>(), Arg.Any<string>()).Throws(new ArgumentException("Bad request"));
        
        //Act
        var response = await _sut.GetSavedMovie(_request,0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMovieEndpoint_MovieNotFound_ReturnsSC404()
    {
        //Arrange
        _savedMovieService.GetSavedMovieOrDefault(Arg.Any<int>(), Arg.Any<string>()).Throws(new NotFoundException("Saved movie not found"));
        
        //Act
        var response = await _sut.GetSavedMovie(_request,0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMovieEndpoint_MovieReturnedNull_ReturnsSC404()
    {
        //Arrange
        _savedMovieService.GetSavedMovieOrDefault(Arg.Any<int>(), Arg.Any<string>()).Returns((SavedMovieDto)null!);
        
        //Act
        var response = await _sut.GetSavedMovie(_request,0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMovieEndpoint_ValidRequest_ReturnsSC200()
    {
        //Arrange
        var savedMovieDto = new SavedMovieDto(1, false);
        _savedMovieService.GetSavedMovieOrDefault(Arg.Any<int>(), Arg.Any<string>()).Returns(savedMovieDto);
        
        //Act
        var response = await _sut.GetSavedMovie(_request,0, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(savedMovieDto, result.Value);
    }
}