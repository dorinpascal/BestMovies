using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.SavedMovieFunctionsTest.SaveMovieEndpointTest;

public class SaveMovieEndpointTest
{
    private readonly DefaultHttpRequest _request;
    private readonly ISavedMovieService _savedMovieService;
    private readonly MockLogger<SavedMovieFunctions> _logger;
    private readonly SavedMovieFunctions _sut;
    
    public SaveMovieEndpointTest()
    {
        var savedMovieDto = new SavedMovieDto(1, false);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(savedMovieDto)))
        };
        _request.Headers.Add("x-ms-client-principal","ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9");
        _savedMovieService = Substitute.For<ISavedMovieService>();
        _logger = Substitute.For<MockLogger<SavedMovieFunctions>>();
        _sut = new SavedMovieFunctions(_savedMovieService);
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_BestMoviesApi_NotAvailable()
    {
        //Arrange
        _savedMovieService.SaveMovie(Arg.Any<SavedMovieDto>(), Arg.Any<CreateUserDto>()).Throws(new Exception());
        
        //Act
        var response = await _sut.SaveMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_MovieAlreadyExists_DuplicateException()
    {
        //Arrange
        _savedMovieService.SaveMovie(Arg.Any<SavedMovieDto>(), Arg.Any<CreateUserDto>()).Throws(new DuplicateException("Movie already exists"));
        
        //Act
        var response = await _sut.SaveMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(409, result.StatusCode);
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_UserUnauthorized_UnauthorizedResult()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");
        
        //Act
        var response = await _sut.SaveMovie(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
}