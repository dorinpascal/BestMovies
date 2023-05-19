using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using Microsoft.Extensions.Primitives;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Api.Test.FunctionTests.ReviewFunctionTest.GetUserReviewForMovie;

public class GetUserReviewForMovieEndpointTests
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ReviewFunctions _sut;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly DefaultHttpRequest _request;

    public GetUserReviewForMovieEndpointTests()
    {
        _reviewRepository = Substitute.For<IReviewRepository>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewRepository);
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(""))),
            Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "userId", "user_id" }
                })
        };
    }

    [Fact]
    public async Task GetUserReviewForMovie_ReviewRepositoryThrowsArgumentException_ReturnsSC400()
    {
        string expectedExceptionMessage = "exception";
        //Arrange
        _reviewRepository.GetUserReviewForMovie(Arg.Any<int>(), Arg.Any<string>()).Throws(new ArgumentException(expectedExceptionMessage));

        //Act
        var response = await _sut.GetUserReviewForMovie(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }


    [Fact]
    public async Task GetUserReviewForMovie_ReviewRepositoryThrowsNotFoundException_ReturnsSC404()
    {
        string expectedExceptionMessage = "exception";
        //Arrange
        _reviewRepository.GetUserReviewForMovie(Arg.Any<int>(), Arg.Any<string>()).Throws(new NotFoundException(expectedExceptionMessage));

        //Act
        var response = await _sut.GetUserReviewForMovie(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expectedExceptionMessage, result.Content);
    }


    [Fact]
    public async Task GetUserReviewForMovie_ReviewRepositoryThrowsError_ReturnsSC500()
    {
        //Arrange
        _reviewRepository.GetUserReviewForMovie(Arg.Any<int>(),Arg.Any<string>()).Throws(new Exception());

        //Act
        var response = await _sut.GetUserReviewForMovie(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
}
