namespace MacheteBang.BookLending.Books.ValueObjects;

public sealed record Isbn
{
    private Isbn(string value) => Value = value;

    public string Value { get; }

    public static Isbn Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("ISBN cannot be null or empty.", nameof(value));
        }

        // Remove any hyphens or spaces for validation
        string cleanedValue = value.Replace("-", "").Replace(" ", "");

        if (cleanedValue.Length != 10 && cleanedValue.Length != 13)
        {
            throw new ArgumentException("ISBN must be either 10 or 13 characters.", nameof(value));
        }

        // Additional validation could be added here, such as checksum validation for ISBN-10 and ISBN-13

        return new Isbn(cleanedValue);
    }

    public override string ToString() => Value;
}
