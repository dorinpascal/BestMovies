using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.Dtos.Movies;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.SavedMovieFunctionsTest.UpdateSavedMovieEndpointTest;

public class UpdateSavedMovieEndpointTest
{
    private DefaultHttpRequest _request;
    private readonly ISavedMovieService _savedMovieService;
    private readonly MockLogger<SavedMovieFunctions> _logger;
    private readonly SavedMovieFunctions _sut;

    private const string ValidHeader =
        "ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9";

    public UpdateSavedMovieEndpointTest()
    {
        var savedMovieDto = new SavedMovieDto(1, false);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(savedMovieDto)))
        };
        _request.Headers.Add("x-ms-client-principal",ValidHeader);
        _savedMovieService = Substitute.For<ISavedMovieService>();
        _logger = Substitute.For<MockLogger<SavedMovieFunctions>>();
        _sut = new SavedMovieFunctions(_savedMovieService);
    }

    [Fact]
    public async Task UpdateSavedMovieEndpoint_BestMoviesApiNotAvailable_ReturnsSC500()
    {
        //Arrange
        _savedMovieService.UpdateMovie(Arg.Any<SavedMovieDto>(), Arg.Any<string>()).Throws(new Exception());
        
        //Act
        var response = await _sut.UpdateSavedMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_UserUnauthorized_UnauthorizedResult()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");
        
        //Act
        var response = await _sut.UpdateSavedMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_BodyParameterIsNotProvided_BadRequestResult()
    {
        //Arrange
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };
        _request.Headers.Add("x-ms-client-principal",ValidHeader);

        
        //Act
        var response = await _sut.SaveMovie(_request,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task UpdateSavedMoveEndpoint_SavedMovieDtoIsInvalid_ArgumentException()
    {
        //Arrange
        var savedMovieDto = new SavedMovieDto(-1, false);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(savedMovieDto)))
        };
        _request.Headers.Add("x-ms-client-principal",ValidHeader);
        _savedMovieService.UpdateMovie(Arg.Any<SavedMovieDto>(), Arg.Any<string>()).Throws(new ArgumentException("SavedMovieDto is invalid"));
        
        //Act
        var response = await _sut.UpdateSavedMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}