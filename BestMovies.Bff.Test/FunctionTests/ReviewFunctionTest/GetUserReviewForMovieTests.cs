using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.ReviewFunctionTest;

public class GetUserReviewForMovieTests
{
    private readonly DefaultHttpRequest _request;
    private readonly IReviewService _reviewService;
    private readonly IUserService _userService;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly ReviewFunctions _sut;
    
    public GetUserReviewForMovieTests()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _request.Headers.Add("x-ms-client-principal","ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9");
        _reviewService = Substitute.For<IReviewService>();
        _userService = Substitute.For<IUserService>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewService, _userService);
    }
    
    [Fact]
    public async Task GetUserReviewForMovieEndpoint_BestMoviesApi_NotAvailable()
    {
        //Arrange
        _reviewService.GetUserReviewForMovie(Arg.Any<int>(), Arg.Any<string>()).Throws(new Exception());
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email@email.com"));;
        
        //Act
        var response = await _sut.GetUserReviewForMovie(_request,"email@email.com",1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task GetUserReviewForMovieEndpoint_ReviewNotFound_ReturnsSC404()
    {
        //Arrange
        _reviewService.GetUserReviewForMovie(Arg.Any<int>(), Arg.Any<string>())
            .Throws(new NotFoundException("Review not found"));
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email@email.com"));

        //Act
        var response = await _sut.GetUserReviewForMovie(_request, "email", 1, _logger);
        var result = (ContentResult) response;

        //Assert
        Assert.Equal(404, result.StatusCode);
    }
    
    [Fact]
    public async Task GetUserReviewForMovieEndpoint_ArgumentException_ReturnsSC400()
    {
        //Arrange
        _reviewService.GetUserReviewForMovie(Arg.Any<int>(), Arg.Any<string>())
            .Throws(new ArgumentException("Invalid argument"));
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email"));

        //Act
        var response = await _sut.GetUserReviewForMovie(_request, "email", 1, _logger);
        var result = (ContentResult) response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetUserReviewForMovieEndpoint_ValidRequest_ReturnsSC200()
    {
        //Arrange
        var review = new ReviewDto(1, new("email@email.com"), 1, "Review");
        _reviewService.GetUserReviewForMovie(Arg.Any<int>(), Arg.Any<string>())
            .Returns(review);
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email"));
    
        //Act
        var response = await _sut.GetUserReviewForMovie(_request, "email", 1, _logger);
        var result = (OkObjectResult) response;
    
        //Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(review, result.Value);
    }
    
    [Fact]
    public async Task GetUserReviewForMovieEndpoint_MovieIdLessThanOne_ReturnsSC400()
    {
        //Arrange
      
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email"));
    
        //Act
        var response = await _sut.GetUserReviewForMovie(_request, "email", 0, _logger);
        var result = (ContentResult) response;
    
        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task GetUserReviewForMovieEndpoint_UserIsNull_ReturnsSc404()
    {
        //Arrange
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns((UserDto)null);
    
        //Act
        var response = await _sut.GetUserReviewForMovie(_request, "email", 1, _logger);
        var result = (ContentResult) response;
    
        //Assert
        Assert.Equal(404, result.StatusCode);
    }
    
    
}