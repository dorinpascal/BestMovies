using BestMovies.Api.Functions;
using BestMovies.Api.Repositories;
using BestMovies.Api.Test.Helpers;
using Microsoft.Extensions.Primitives;

namespace BestMovies.Api.Test.FunctionTests.ReviewFunctionTest.GetUserReviewForMovie;

public class ParametersTests
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ReviewFunctions _sut;
    private readonly MockLogger<ReviewFunctions> _logger;


    public ParametersTests()
    {
        _reviewRepository = Substitute.For<IReviewRepository>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewRepository);
    }

    [Fact]
    public async Task GetUserReviewForMovie_UserIdIsNotProvidedInQuery_ReturnsBadRequest()
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };

        //Act
        var response = await _sut.GetUserReviewForMovie(request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetUserReviewForMovie_MovieIdHasWrongValue_ReturnsBadRequest(int movieId)
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(""))),
            Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "userId", "user_id" }
                })
        };

        //Act
        var response = await _sut.GetUserReviewForMovie(request, movieId, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }

}
