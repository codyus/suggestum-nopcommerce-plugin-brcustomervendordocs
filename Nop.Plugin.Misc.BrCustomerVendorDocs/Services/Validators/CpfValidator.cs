using System.Text.RegularExpressions;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;

/// <summary>
/// CPF validator
/// </summary>
public static class CpfValidator
{
    private static readonly Regex CpfRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    /// <summary>
    /// Normalizes CPF by removing all non-digit characters
    /// </summary>
    /// <param name="cpf">CPF to normalize</param>
    /// <returns>Normalized CPF (digits only)</returns>
    public static string Normalize(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        return Regex.Replace(cpf, @"[^\d]", string.Empty);
    }

    /// <summary>
    /// Validates CPF format and check digits
    /// </summary>
    /// <param name="cpf">CPF to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var normalized = Normalize(cpf);

        // Must be exactly 11 digits
        if (!CpfRegex.IsMatch(normalized))
            return false;

        // Check for invalid sequences (all same digit)
        if (normalized.All(c => c == normalized[0]))
            return false;

        // Validate check digits
        return ValidateCheckDigits(normalized);
    }

    /// <summary>
    /// Validates CPF check digits
    /// </summary>
    private static bool ValidateCheckDigits(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        var digits = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        // Calculate first check digit
        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += digits[i] * (10 - i);
        }

        var remainder = sum % 11;
        var firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        if (digits[9] != firstCheckDigit)
            return false;

        // Calculate second check digit
        sum = 0;
        for (var i = 0; i < 10; i++)
        {
            sum += digits[i] * (11 - i);
        }

        remainder = sum % 11;
        var secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return digits[10] == secondCheckDigit;
    }
}

