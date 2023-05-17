using BestMovies.Api.Functions;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.ReviewFunctionTest.GetReviewsForMovieEndpoint;

public class GetReviewsForMovieEndpointTests
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ReviewFunctions _sut;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly DefaultHttpRequest _request;
    public GetReviewsForMovieEndpointTests()
    {
        _reviewRepository = Substitute.For<IReviewRepository>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext());
    }


    [Fact]
    public async Task GetReviewsForMovieEndpoint_ReviewRepositoryThrowsError_ReturnsSC500()
    {
        //Arrange
        _reviewRepository.GetReviewsForMovie(Arg.Any<int>()).Throws(new Exception());

        //Act
        var response = await _sut.GetReviewsForMovie(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task GetReviewsForMovieEndpoint_ReturnsListOfReviews_ReturnsSC200()
    {
        //Arrange
        var reviews = new List<Review>() { new Review(1,"userId",1,"comment") };

        _reviewRepository.GetReviewsForMovie(Arg.Any<int>()).Returns(reviews);

        //Act
        var response = await _sut.GetReviewsForMovie(_request, 1, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }

}
