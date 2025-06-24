namespace MacheteBang.BookLending.Books.Tests.Unit;

public class IsbnTests
{
    [Fact]
    public void Create_ShouldCreateIsbn_WhenValueIsValid()
    {
        // Arrange
        string validIsbn10 = "0-306-40615-2";
        string validIsbn13 = "978-3-16-148410-0";

        // Act
        var isbn10Result = Isbn.Create(validIsbn10);
        var isbn13Result = Isbn.Create(validIsbn13);

        // Assert
        isbn10Result.ShouldNotBeNull();
        isbn10Result.Value.ShouldBe(validIsbn10);

        isbn13Result.ShouldNotBeNull();
        isbn13Result.Value.ShouldBe(validIsbn13);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldThrowArgumentException_WhenValueIsNullOrEmpty(string? invalidValue)
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => Isbn.Create(invalidValue));
        exception.Message.ShouldContain("ISBN cannot be null or empty.");
        exception.ParamName.ShouldBe("value");
    }
    [Theory]
    [InlineData("12345")]
    [InlineData("123456789012")]
    [InlineData("12345678901234")]
    public void Create_ShouldThrowArgumentException_WhenFormatIsInvalid(string invalidValue)
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => Isbn.Create(invalidValue));
        exception.Message.ShouldContain("Invalid ISBN format or checksum.");
        exception.ParamName.ShouldBe("value");
    }

    [Theory]
    [InlineData("0306406152")] // Valid ISBN-10
    [InlineData("0-306-40615-2")] // Valid ISBN-10 with hyphens
    [InlineData("9780306406157")] // Valid ISBN-13
    [InlineData("978-0-306-40615-7")] // Valid ISBN-13 with hyphens
    public void IsValidISBN_ShouldReturnTrue_ForValidISBN(string validIsbn)
    {
        // Act
        var result = Isbn.IsValidISBN(validIsbn);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("0306406151")] // Invalid ISBN-10 (wrong check digit)
    [InlineData("9780306406151")] // Invalid ISBN-13 (wrong check digit)
    [InlineData("03064061X1")] // Too long for ISBN-10
    [InlineData("978030640615")] // Too short for ISBN-13
    [InlineData("ABCDEFGHIJK")] // Not a number
    public void IsValidISBN_ShouldReturnFalse_ForInvalidISBN(string invalidIsbn)
    {
        // Act
        var result = Isbn.IsValidISBN(invalidIsbn);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var isbn = Isbn.Create("978-3-16-148410-0");

        // Act
        var result = isbn.ToString();

        // Assert
        result.ShouldBe("978-3-16-148410-0");
    }

    [Theory]
    [InlineData("123456789X")] // Valid ISBN-10 with X check digit
    public void IsValidISBN_ShouldAcceptXAsCheckDigit_ForISBN10(string isbnWithX)
    {
        // Act
        var result = Isbn.IsValidISBN(isbnWithX);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("0306406151")] // Invalid ISBN-10 (wrong check digit)
    [InlineData("9780306406151")] // Invalid ISBN-13 (wrong check digit)
    [InlineData("abc1234567")] // Invalid characters in ISBN-10
    [InlineData("9781234567abc")] // Invalid characters in ISBN-13
    public void Create_ShouldThrowArgumentException_WhenChecksumIsInvalid(string invalidValue)
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => Isbn.Create(invalidValue));
        exception.Message.ShouldContain("Invalid ISBN format or checksum.");
        exception.ParamName.ShouldBe("value");
    }

    [Fact]
    public void Create_ShouldTrimInput_BeforeValidating()
    {
        // Arrange
        string validIsbnWithWhitespace = "  978-3-16-148410-0  ";
        string expectedTrimmedValue = "978-3-16-148410-0";

        // Act
        var result = Isbn.Create(validIsbnWithWhitespace);

        // Assert
        result.Value.ShouldBe(expectedTrimmedValue);
    }
}
