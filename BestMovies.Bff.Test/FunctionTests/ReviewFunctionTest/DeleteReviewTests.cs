using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.ReviewFunctionTest;

public class DeleteReviewTests
{
    private readonly DefaultHttpRequest _request;
    private readonly IReviewService _reviewService;
    private readonly IUserService _userService;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly ReviewFunctions _sut;
    
    public DeleteReviewTests()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _request.Headers.Add("x-ms-client-principal","ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9");
        _reviewService = Substitute.For<IReviewService>();
        _userService = Substitute.For<IUserService>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewService, _userService);
    }
    
    [Fact]
    public async Task DeleteReviewEndpoint_BestMoviesApi_NotAvailable()
    {
        //Arrange
        _reviewService.DeleteReview(Arg.Any<int>(), Arg.Any<string>()).Throws(new Exception());
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email"));

        //Act
        var response = await _sut.DeleteReview(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteReviewEndpoint_ArgumentException_ReturnsSC400()
    {
        //Arrange
        _reviewService.DeleteReview(Arg.Any<int>(), Arg.Any<string>())
            .Throws(new ArgumentException("Review not found"));
        _userService.GetUserOrDefault(Arg.Any<string>()).Returns(new UserDto("1", "email"));

        //Act
        var response = await _sut.DeleteReview(_request, 1, _logger);
        var result = (ContentResult) response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteReviewEndpoint_UserNotAuthenticated_ReturnsSC401()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");
        
        //Act
        var response = await _sut.DeleteReview(_request, 1, _logger);
        var result = (ContentResult) response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteReviewEndpoint_MovieIdLessThanOne_ReturnsSC400()
    {
        //Arrange
        
        //Act
        var response = await _sut.DeleteReview(_request, 0, _logger);
        var result = (ContentResult) response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteReviewEndpoint_ValidRequest_ReturnsSC200()
    {
        //Arrange

        //Act
        var response = await _sut.DeleteReview(_request, 1, _logger);
        var result = (StatusCodeResult) response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
}