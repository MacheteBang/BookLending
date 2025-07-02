using Microsoft.AspNetCore.Mvc;

namespace MacheteBang.BookLending.Tests.Integration.UsersTests;

public class RegisterTests(BookLendingWebApplicationFactory factory) : IClassFixture<BookLendingWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    private RegisterUserRequest _request = default!;
    private HttpResponseMessage _apiResponse = default!;

    [Fact]
    public void RegisterValidUser_ShouldReturnSuccess()
    {
        GivenValidUserRequest();
        WhenUserSubmitsRegistration();
        ThenUserCreatedSuccessfully();
    }

    [Fact(Skip = "Test not implemented yet")]

    public void RegisterUserWithInvalidEmail_ShouldReturnBadRequest()
    {
        // GivenUserRequestWithInvalidEmail();
        // WhenUserSubmitsRegistration();
        // ThenResponseShouldBeBadRequest();
    }

    [Fact(Skip = "Test not implemented yet")]
    public void RegisterUserWithShortPassword_ShouldReturnBadRequest()
    {
        // GivenUserRequestWithShortPassword();
        // WhenUserSubmitsRegistration();
        // ThenResponseShouldBeBadRequest();
    }

    [Fact(Skip = "Test not implemented yet")]
    public void RegisterUserWithWeakPassword_ShouldReturnBadRequest()
    {
        // GivenUserRequestWithWeakPassword();
        // WhenUserSubmitsRegistration();
        // ThenResponseShouldBeBadRequest();
    }

    [Fact]
    public void RegisterSameUserTwice_SecondAttemptShouldReturnConflict()
    {
        GivenValidUserRequest();
        WhenUserSubmitsRegistration();
        ThenUserCreatedSuccessfully();
        WhenUserSubmitsRegistration();
        ThenResponseShouldBeDuplicateUsernameConflict();
    }

    private void GivenValidUserRequest()
    {
        _request = new RegisterUserRequest
        (
            Email: "test@example.com",
            Password: "Pass@word1"
        );
    }

    private void WhenUserSubmitsRegistration()
    {
        var content = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
        _apiResponse = _client.PostAsync("/users/register", content).GetAwaiter().GetResult();
    }
    private void ThenUserCreatedSuccessfully()
    {
        // Assert HTTP status code
        _apiResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        // Assert Location header
        _apiResponse.Headers.ShouldContain(h => h.Key == "Location", "Response should contain a Location header");

        var locationHeader = _apiResponse.Headers.GetValues("Location").First();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain("/users/", Insensitive);

        // Assert response body
        string body = _apiResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        body.ShouldNotBeNullOrEmpty();
        UserResponse? response = JsonSerializer.Deserialize<UserResponse>(body, _jsonOptions);

        response.ShouldNotBeNull();
        response.Email.ShouldBe(_request.Email);
    }
    private void ThenResponseShouldBeDuplicateUsernameConflict()
    {
        _apiResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        string body = _apiResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        body.ShouldNotBeNullOrEmpty();
        ProblemDetails? problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body, _jsonOptions);
        problemDetails.ShouldNotBeNull();
        problemDetails.Title.ShouldBe("Users.DuplicateUserName");
    }
}
