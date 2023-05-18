using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.UserFunctionTest.SaveUserEndpoint;

public class SaveUserEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly UsersFunctions _sut;
    private readonly MockLogger<UsersFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public SaveUserEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _logger = Substitute.For<MockLogger<UsersFunctions>>();
        _sut = new UsersFunctions(_userRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new CreateUserDto("id", "email"))))
        };
    }

    [Fact]
    public async Task SaveUserEndpoint_UserRepositoryThrowsDuplicateException_ReturnsSC409()
    {
        string expectedExceptionMessage = "exception";
        //Arrange
        _userRepository.SaveUser(Arg.Any<string>(), Arg.Any<string>()).Throws(new DuplicateException(expectedExceptionMessage));

        //Act
        var response = await _sut.SaveUser(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(409, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }

    [Fact]
    public async Task SaveUserEndpoint_UserRepositoryThrowsArgumentException_ReturnsSC400()
    {
        string expectedExceptionMessage = "exception";
        //Arrange
        _userRepository.SaveUser(Arg.Any<string>(), Arg.Any<string>()).Throws(new ArgumentException(expectedExceptionMessage));

        //Act
        var response = await _sut.SaveUser(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }
    
    [Fact]
    public async Task SaveUserEndpoint_UserRepositoryThrowsException_ReturnsSC500()
    {
        string expectedExceptionMessage = "The server is temporarily unable to handle the request, please try again later!";
        //Arrange
        _userRepository.SaveUser(Arg.Any<string>(), Arg.Any<string>()).Throws(new Exception());

        //Act
        var response = await _sut.SaveUser(_request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }

    [Fact]
    public async Task SaveUserEndpoint_UserWasAddedWithSuccess_ReturnsSC200()
    {
        //Arrange
        _userRepository.SaveUser(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);

        //Act
        var response = await _sut.SaveUser(_request, _logger);
        var result = (OkResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode); 
    }

}
