namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Models;

/// <summary>
/// Model for vendor CNPJ data in Admin
/// </summary>
public class VendorCnpjModel
{
    /// <summary>
    /// Gets or sets the CNPJ value
    /// </summary>
    public string Cnpj { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether CNPJ is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether alphanumeric CNPJ is allowed
    /// </summary>
    public bool AllowAlphaNumeric { get; set; } = true;
}

