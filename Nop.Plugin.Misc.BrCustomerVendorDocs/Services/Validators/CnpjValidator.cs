using System.Text.RegularExpressions;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;

/// <summary>
/// CNPJ validator
/// </summary>
public static class CnpjValidator
{
    private static readonly Regex NumericCnpjRegex = new(@"^\d{14}$", RegexOptions.Compiled);
    private static readonly Regex AlphaNumericCnpjRegex = new(@"^[A-Z0-9]{14}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Normalizes CNPJ by removing punctuation and converting to uppercase
    /// </summary>
    /// <param name="cnpj">CNPJ to normalize</param>
    /// <returns>Normalized CNPJ</returns>
    public static string Normalize(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return string.Empty;

        // Remove punctuation (. / - and spaces)
        var normalized = Regex.Replace(cnpj, @"[.\s/-]", string.Empty);

        // Convert to uppercase
        return normalized.ToUpperInvariant();
    }

    /// <summary>
    /// Validates CNPJ format
    /// </summary>
    /// <param name="cnpj">CNPJ to validate</param>
    /// <param name="allowAlphaNumeric">Whether to allow alphanumeric format</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValid(string cnpj, bool allowAlphaNumeric = true)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        var normalized = Normalize(cnpj);

        // Must be exactly 14 characters
        if (normalized.Length != 14)
            return false;

        // Check if numeric
        if (NumericCnpjRegex.IsMatch(normalized))
        {
            // Check for invalid sequences (all same digit)
            if (normalized.All(c => c == normalized[0]))
                return false;

            // Validate check digits for numeric CNPJ
            return ValidateNumericCheckDigits(normalized);
        }

        // Check if alphanumeric (if allowed)
        if (allowAlphaNumeric && AlphaNumericCnpjRegex.IsMatch(normalized))
        {
            // Check for invalid sequences (all same character)
            if (normalized.All(c => c == normalized[0]))
                return false;

            // For alphanumeric, only validate format and length
            // No check digits validation (no stable public rule)
            return true;
        }

        return false;
    }

    /// <summary>
    /// Validates numeric CNPJ check digits
    /// </summary>
    private static bool ValidateNumericCheckDigits(string cnpj)
    {
        if (cnpj.Length != 14)
            return false;

        var digits = cnpj.Select(c => int.Parse(c.ToString())).ToArray();

        // Calculate first check digit
        var weights1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum = 0;
        for (var i = 0; i < 12; i++)
        {
            sum += digits[i] * weights1[i];
        }

        var remainder = sum % 11;
        var firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        if (digits[12] != firstCheckDigit)
            return false;

        // Calculate second check digit
        var weights2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        sum = 0;
        for (var i = 0; i < 13; i++)
        {
            sum += digits[i] * weights2[i];
        }

        remainder = sum % 11;
        var secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return digits[13] == secondCheckDigit;
    }
}

