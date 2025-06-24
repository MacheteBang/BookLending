using System.Text.RegularExpressions;

namespace MacheteBang.BookLending.Books.ValueObjects;

public sealed record Isbn
{
    private Isbn(string value) => Value = value;

    public string Value { get; }

    public static Isbn Create(string? value)
    {
        string trimmedValue = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(trimmedValue))
        {
            throw new ArgumentException("ISBN cannot be null or empty.", nameof(value));
        }

        if (!IsValidISBN(trimmedValue))
        {
            throw new ArgumentException("Invalid ISBN format or checksum.", nameof(value));
        }
        return new Isbn(trimmedValue);
    }

    public override string ToString() => Value;

    public static bool IsValidISBN(string isbn)
    {
        // Remove hyphens for easier processing
        isbn = isbn.Replace("-", "");

        if (isbn.Length == 10)
        {
            return IsValidISBN10(isbn);
        }
        else if (isbn.Length == 13)
        {
            return IsValidISBN13(isbn);
        }
        else
        {
            return false; // Not a valid length
        }
    }

    private static bool IsValidISBN10(string isbn)
    {
        //Regex for ISBN-10: nine digits or an 'X' as the last digit
        if (!Regex.IsMatch(isbn, @"^\d{9}[\dX]$"))
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (10 - i) * int.Parse(isbn[i].ToString());
        }

        char lastDigit = isbn[9];
        int checkDigit = (lastDigit == 'X') ? 10 : int.Parse(lastDigit.ToString());
        sum += checkDigit;

        return sum % 11 == 0;
    }

    private static bool IsValidISBN13(string isbn)
    {
        //Regex for ISBN-13: thirteen digits
        if (!Regex.IsMatch(isbn, @"^\d{13}$"))
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(isbn[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int checkDigit = 10 - (sum % 10);
        if (checkDigit == 10)
        {
            checkDigit = 0;
        }

        return checkDigit == int.Parse(isbn[12].ToString());
    }
}
