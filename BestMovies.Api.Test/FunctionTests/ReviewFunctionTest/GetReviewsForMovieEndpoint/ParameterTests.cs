using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;

namespace BestMovies.Api.Test.FunctionTests.ReviewFunctionTest.GetReviewsForMovieEndpoint;

public class ParameterTests
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ReviewFunctions _sut;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly DefaultHttpRequest _request;
    public ParameterTests()
    {
        _reviewRepository = Substitute.For<IReviewRepository>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetReviewsForMovieEndpoint_UserIdIsNotProvided_ReturnsBadRequest(int id)
    {
        //Arrange
        //Act
        var response = await _sut.GetReviewsForMovie(_request,id, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}
