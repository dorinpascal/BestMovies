using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.ReviewFunctionTest.AddReviewEndpoint;

public class AddReviewEndpointTests
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ReviewFunctions _sut;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly DefaultHttpRequest _request;
    public AddReviewEndpointTests()
    {
        _reviewRepository = Substitute.For<IReviewRepository>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ReviewDto(1, new UserDto("email"), 1, "comment"))))
        };
    }

    [Fact]
    public async Task AddReviewEndpoint_ReviewRepositoryThrowsError_ReturnsSC500()
    {
        //Arrange
        _reviewRepository.CreateReview(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Throws(new Exception());

        //Act
        var response = await _sut.AddReview(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task AddReviewEndpoint_ReviewRepositoryThrowsArgumentException_ReturnsSC500()
    {
        //Arrange
        _reviewRepository.CreateReview(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Throws(new ArgumentException());

        //Act
        var response = await _sut.AddReview(_request, "userId", _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task AddReviewEndpoint_ReviewWasAddedWithSuccess_ReturnsSC200()
    {
        //Arrange
        _reviewRepository.CreateReview(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(Task.CompletedTask);

        //Act
        var response = await _sut.AddReview(_request, "userId", _logger);
        var result = (OkResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
}
