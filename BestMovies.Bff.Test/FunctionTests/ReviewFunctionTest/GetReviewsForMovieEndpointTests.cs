using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.ReviewFunctionTest;

public class GetReviewsForMovieEndpointTests
{
    //make this follow similar structure to the AddReviewEndpointTests class
    private readonly DefaultHttpRequest _request;
    private readonly IReviewService _reviewService;
    private readonly IUserService _userService;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly ReviewFunctions _sut;
    
    public GetReviewsForMovieEndpointTests()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _request.Headers.Add("x-ms-client-principal","ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9");
        _reviewService = Substitute.For<IReviewService>();
        _userService = Substitute.For<IUserService>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewService, _userService);
    }
    
    [Fact]
    public async Task GetReviewsForMovieEndpoint_BestMoviesApi_NotAvailable()
    {
        //Arrange
        _reviewService.GetReviewsForMovie(Arg.Any<int>()).Throws(new Exception());
        
        //Act
        var response = await _sut.GetReviewsForMovie(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task GetReviewsForMovieEndpoint_ThrowsArgumentException_BadRequest()
    {
        //Arrange
        _reviewService.GetReviewsForMovie(Arg.Any<int>()).Throws(new ArgumentException());
        
        //Act
        var response = await _sut.GetReviewsForMovie(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetReviewsForMovieEndpoint_ValidRequest_ReturnsSC200()
    {
        //Arrange
        var reviews = new List<ReviewDto>()
        {
            new(1, new SimpleUserDto("testemail@test.com"), 5, "Comment")
        };
        _reviewService.GetReviewsForMovie(Arg.Any<int>()).Returns(reviews);
        
        //Act
        var response = await _sut.GetReviewsForMovie(_request,1, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(reviews, result.Value);
    }
    
    [Fact]
    public async Task GetReviewForMovieEndpoint_IdLessThanOne_ReturnsSC400()
    {
        //Arrange
        
        //Act
        var response = await _sut.GetReviewsForMovie(_request,0, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    
    
   
}