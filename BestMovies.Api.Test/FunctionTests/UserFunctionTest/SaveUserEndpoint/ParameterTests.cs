using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;

namespace BestMovies.Api.Test.FunctionTests.UserFunctionTest.SaveUserEndpoint;

public class ParameterTests
{
    private readonly IUserRepository _userRepository;
    private readonly UsersFunctions _sut;
    private readonly MockLogger<UsersFunctions> _logger;

    public ParameterTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _logger = Substitute.For<MockLogger<UsersFunctions>>();
        _sut = new UsersFunctions(_userRepository);
    }

    [Fact]
    public async Task SaveUserEndpoint_BodyParametersIsNotProvided_ReturnsBadRequest()
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };

        //Act
        var response = await _sut.SaveUser(request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}
