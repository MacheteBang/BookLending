namespace MacheteBang.BookLending.Kernel.Tests.Unit;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name@example.com")]
    [InlineData("user+tag@example.com")]
    [InlineData("user123@example.co.uk")]
    [InlineData("user-name@example-site.com")]
    [InlineData("user_name@example.org")]
    [InlineData("user@subdomain.example.com")]
    [InlineData("USER@EXAMPLE.COM")] // Testing case insensitivity
    public void IsValidEmail_ShouldReturnTrue_ForValidEmails(string validEmail)
    {
        // Act
        var result = validEmail.IsValidEmail();

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("userexample.com")] // Missing @ symbol
    [InlineData("user@")] // Missing domain
    [InlineData("@example.com")] // Missing username
    [InlineData("user@example")] // Missing TLD
    [InlineData("user@.com")] // Missing domain name
    [InlineData("user@example..com")] // Double dot in domain
    [InlineData("user name@example.com")] // Space in username
    [InlineData("user@exam ple.com")] // Space in domain
    [InlineData("user@example.com.")] // Trailing dot
    [InlineData("user@-example.com")] // Domain starts with hyphen
    [InlineData("user@example-.com")] // Domain part ends with hyphen
    public void IsValidEmail_ShouldReturnFalse_ForInvalidEmails(string invalidEmail)
    {
        // Act
        var result = invalidEmail.IsValidEmail();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsValidEmail_ShouldReturnFalse_ForNullEmail()
    {
        // Arrange
        string? nullEmail = null;

        // Act
        var result = StringExtensions.IsValidEmail(nullEmail!);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsValidEmail_ShouldHandleEdgeCases()
    {
        // Arrange
        string longLocalPart = new string('a', 64) + "@example.com";
        string longDomainPart = "user@" + new string('a', 63) + ".com";
        string emailWithSpecialChars = "user!#$%&'*+-/=?^_`{|}~@example.com";

        // Act & Assert
        longLocalPart.IsValidEmail().ShouldBeTrue();
        longDomainPart.IsValidEmail().ShouldBeTrue();
        emailWithSpecialChars.IsValidEmail().ShouldBeTrue();
    }
}
