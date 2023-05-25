using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.SavedMovieFunctionsTest.SaveMovieEndpointTest;

public class ParameterTest
{
    private readonly ISavedMovieService _savedMovieService;
    private readonly IUserService _userService;
    private readonly MockLogger<SavedMovieFunctions> _logger;

    private const string InvalidHeader = "ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIiLAogICJ1c2VyRGV0YWlscyI6ICJlbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9";

    private const string ValidHeader = "ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9";

    public ParameterTest()
    {
        _savedMovieService = Substitute.For<ISavedMovieService>();
        _logger = Substitute.For<MockLogger<SavedMovieFunctions>>();
        _userService = Substitute.For<IUserService>();
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_BodyParameter_IsNotProvided()
    {
        //Arrange
        var function = new SavedMovieFunctions(_userService, _savedMovieService);
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };
        request.Headers.Add("x-ms-client-principal",ValidHeader);

        
        //Act
        var response = await function.SaveMovie(request,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task SaveMovieEndpoint_HeaderParameterIsInvalid_UnauthorizedResult()
    {
        //Arrange
        var function = new SavedMovieFunctions(_userService, _savedMovieService);
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new SavedMovieDto(1, false))))
        };
        request.Headers.Add("x-ms-client-principal",InvalidHeader);
        
        //Act
        var response = await function.SaveMovie(request,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task SaveMoveEndpoint_SavedMovieDtoIsInvalid_ArgumentException()
    {
        //Arrange
        var function = new SavedMovieFunctions(_userService, _savedMovieService);
        _savedMovieService.SaveMovie(Arg.Any<SavedMovieDto>(), Arg.Any<UserDto>()).Throws(new ArgumentException("SavedMovieDto is invalid"));
        
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new SavedMovieDto(-1, false))))
        };
        request.Headers.Add("x-ms-client-principal", ValidHeader);
        
        //Act
        var response = await function.SaveMovie(request,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}