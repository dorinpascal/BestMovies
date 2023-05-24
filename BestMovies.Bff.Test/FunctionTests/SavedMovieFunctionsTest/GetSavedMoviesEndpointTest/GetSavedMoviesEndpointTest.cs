using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.SavedMovieFunctionsTest.GetSavedMoviesEndpointTest;

public class GetSavedMoviesEndpointTest
{
    private readonly DefaultHttpRequest _request;
    private readonly ISavedMovieService _savedMovieService;
    private readonly MockLogger<SavedMovieFunctions> _logger;
    private readonly SavedMovieFunctions _sut;
    private const string ValidHeader =
        "ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9";
    
    public GetSavedMoviesEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _request.Headers.Add("x-ms-client-principal", ValidHeader);
        _savedMovieService = Substitute.For<ISavedMovieService>();
        _logger = Substitute.For<MockLogger<SavedMovieFunctions>>();
        _sut = new SavedMovieFunctions(_savedMovieService);
    }
    
    [Fact]
    public async Task GetSavedMoviesEndpoint_BestMoviesApiNotAvailable_ReturnsSC500()
    {
        //Arrange
        _savedMovieService.GetSavedMoviesForUser(Arg.Any<string>(), Arg.Any<bool>()).Throws(new Exception());
        
        //Act
        var response = await _sut.GetSavedMovies(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMoviesEndpoint_UserIsUnauthorized_UnauthorizedResult()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");
        
        //Act
        var response = await _sut.GetSavedMovies(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMoviesEndpoint_ArgumentException_ReturnsSC400()
    {
        //Arrange
        _savedMovieService.GetSavedMoviesForUser(Arg.Any<string>(), Arg.Any<bool>()).Throws(new ArgumentException("Bad request"));
        
        //Act
        var response = await _sut.GetSavedMovies(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetSavedMoviesEndpoint_ValidRequest_ReturnsSC200()
    {
        //Arrange
        //Arrange
        var movies = new List<SearchMovieDto>()
        {
            new(1,"title", new List<string>()
            {
                "genre"
            }),
        };
        _savedMovieService.GetSavedMoviesForUser(Arg.Any<string>(), Arg.Any<bool>()).Returns(movies);
        
        //Act
        var response = await _sut.GetSavedMovies(_request, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(movies, result.Value);
    }
}