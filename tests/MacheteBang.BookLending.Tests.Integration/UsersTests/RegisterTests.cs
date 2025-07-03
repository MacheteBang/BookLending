using Microsoft.AspNetCore.Mvc;

namespace MacheteBang.BookLending.Tests.Integration.UsersTests;

public class RegisterTests()
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    [Fact]
    public void RegisterValidUser_ShouldReturnSuccess()
    {
        var client = GivenNewWebApplication();
        var request = GivenValidUserRequest();

        var response = WhenUserSubmitsRegistration(client, request);

        ThenUserCreatedSuccessfully(response, request);
    }

    [Fact]

    public void RegisterUserWithInvalidEmail_ShouldReturnBadRequest()
    {
        var client = GivenNewWebApplication();
        var request = GivenUserRequestWithInvalidEmail();

        var response = WhenUserSubmitsRegistration(client, request);

        ThenResponseShouldBeInvalidEmailBadResponse(response);
    }

    [Fact]
    public void RegisterUserWithShortPassword_ShouldReturnBadRequest()
    {
        var client = GivenNewWebApplication();
        var request = GivenUserRequestWithShortPassword();

        var response = WhenUserSubmitsRegistration(client, request);

        ThenResponseShouldBeInvalidPasswordBadResponse(response);
    }

    [Fact]
    public void RegisterUserWithWeakPassword_ShouldReturnBadRequest()
    {
        var client = GivenNewWebApplication();
        var request = GivenUserRequestWithWeakPassword();

        var response = WhenUserSubmitsRegistration(client, request);

        ThenResponseShouldBeInvalidPasswordBadResponse(response);
    }

    [Fact]
    public void RegisterSameUserTwice_SecondAttemptShouldReturnConflict()
    {
        var client = GivenNewWebApplication();
        var request = GivenValidUserRequest();

        var response = WhenUserSubmitsRegistration(client, request);
        ThenUserCreatedSuccessfully(response, request);
        response = WhenUserSubmitsRegistration(client, request);
        ThenResponseShouldBeDuplicateUsernameConflict(response);
        ThenResponseShouldBeDuplicateUsernameConflict(response);
    }

    private static RegisterUserRequest GivenValidUserRequest()
    {
        return new RegisterUserRequest
        (
            Email: "test@example.com",
            Password: "Pass@word1"
        );
    }

    private static HttpClient GivenNewWebApplication()
    {
        return new BookLendingWebApplicationFactory(Guid.CreateVersion7().ToString()).CreateClient();
    }

    private static RegisterUserRequest GivenUserRequestWithInvalidEmail()
    {
        return new RegisterUserRequest
        (
            Email: "invalid-email",
            Password: "Pass@word1"
        );
    }

    private static RegisterUserRequest GivenUserRequestWithShortPassword()
    {
        return new RegisterUserRequest
        (
            Email: "test@example.com",
            Password: "short"
        );
    }

    private static RegisterUserRequest GivenUserRequestWithWeakPassword()
    {
        return new RegisterUserRequest
        (
            Email: "test@example.com",
            Password: "weakpassword"
        );
    }

    private static HttpResponseMessage WhenUserSubmitsRegistration(HttpClient client, RegisterUserRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        return client.PostAsync("/users/register", content).GetAwaiter().GetResult();
    }

    private static void ThenUserCreatedSuccessfully(HttpResponseMessage response, RegisterUserRequest expectedRequest)
    {
        // Assert HTTP status code
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        // Assert Location header
        response.Headers.ShouldContain(h => h.Key == "Location", "Response should contain a Location header");

        var locationHeader = response.Headers.GetValues("Location").First();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain("/users/", Insensitive);

        // Assert response body
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        body.ShouldNotBeNullOrEmpty();
        UserResponse? userResponse = JsonSerializer.Deserialize<UserResponse>(body, _jsonOptions);

        userResponse.ShouldNotBeNull();
        userResponse.Email.ShouldBe(expectedRequest.Email);
    }

    private static void ThenResponseShouldBeDuplicateUsernameConflict(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        body.ShouldNotBeNullOrEmpty();
        ProblemDetails? problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body, _jsonOptions);
        problemDetails.ShouldNotBeNull();
        problemDetails.Title.ShouldBe("Users.DuplicateUserName");
    }

    private static void ThenResponseShouldBeInvalidEmailBadResponse(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        body.ShouldNotBeNullOrEmpty();
        ValidationProblemDetails? problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(body, _jsonOptions);
        problemDetails.ShouldNotBeNull();
        problemDetails.Errors.Any(e => e.Key == "Kernel.InvalidEmail").ShouldBeTrue();
    }

    private static void ThenResponseShouldBeInvalidPasswordBadResponse(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        body.ShouldNotBeNullOrEmpty();
        ValidationProblemDetails? problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(body, _jsonOptions);
        problemDetails.ShouldNotBeNull();
        problemDetails.Errors.Any(e => e.Key.Contains("Password")).ShouldBeTrue();
    }
}
