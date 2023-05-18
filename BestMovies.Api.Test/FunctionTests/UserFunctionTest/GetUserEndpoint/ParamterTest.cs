using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;

namespace BestMovies.Api.Test.FunctionTests.UserFunctionTest.GetUserEndpoint;

public class ParamterTest
{
    private readonly IUserRepository _userRepository;
    private readonly UsersFunctions _sut;
    private readonly MockLogger<UsersFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public ParamterTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _logger = Substitute.For<MockLogger<UsersFunctions>>();
        _sut = new UsersFunctions(_userRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetUserEndpoint_QueryParameterIsNotProvided_ReturnsBadRequest(string userId)
    {
        //Arrange
        //Act
        var response = await _sut.GetUser(_request, userId,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}
