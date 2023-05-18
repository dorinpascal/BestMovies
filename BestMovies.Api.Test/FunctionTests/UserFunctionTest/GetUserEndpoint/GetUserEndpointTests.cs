using BestMovies.Api.Functions;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.UserFunctionTest.GetUserEndpoint;

public class GetUserEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly UsersFunctions _sut;
    private readonly MockLogger<UsersFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public GetUserEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _logger = Substitute.For<MockLogger<UsersFunctions>>();
        _sut = new UsersFunctions(_userRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext());
    }

    [Fact]
    public async Task GetUserEndpoint_UserRepositoryThrowsNotFoundException_ReturnsSC400()
    {
        string expectedExceptionMessage = "exception";
        //Arrange
        _userRepository.GetUser(Arg.Any<string>()).Throws(new NotFoundException(expectedExceptionMessage));

        //Act
        var response = await _sut.GetUser(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }

    [Fact]
    public async Task GetUserEndpoint_UserRepositoryThrowsException_ReturnsSC500()
    {
        //Arrange
        string expectedExceptionMessage = "The server is temporarily unable to handle the request, please try again later!";
        _userRepository.GetUser(Arg.Any<string>()).Throws(new Exception());

        //Act
        var response = await _sut.GetUser(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }

    [Fact]
    public async Task GetUserEndpoint_ReturnsUser_ReturnsSC200()
    {
        //Arrange
        var user = new User("userId", "email");

        _userRepository.GetUser(Arg.Any<string>()).Returns(user);

        //Act
        var response = await _sut.GetUser(_request, "userId", _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }

}
