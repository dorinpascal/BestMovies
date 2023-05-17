using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;

namespace BestMovies.Api.Test.FunctionTests.ReviewFunctionTest.AddReviewEndpoint;

public class ParametersTest
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ReviewFunctions _sut;
    private readonly MockLogger<ReviewFunctions> _logger;
    public ParametersTest()
    {
        _reviewRepository = Substitute.For<IReviewRepository>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewRepository);
    }


    [Fact]
    public async Task AddReviewEndpoint_UserIdIsNotProvided_ReturnsBadRequest()
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };

        //Act
        var response = await _sut.AddReview(request, string.Empty, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }


    [Fact]
    public async Task AddReviewEndpoint_BodyParametersIsNotProvided_ReturnsBadRequest()
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };

        //Act
        var response = await _sut.AddReview(request,"userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}



