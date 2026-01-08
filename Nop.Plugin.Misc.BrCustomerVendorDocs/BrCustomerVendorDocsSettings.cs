using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs;

/// <summary>
/// Represents plugin settings
/// </summary>
public class BrCustomerVendorDocsSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether CPF is required for customers
    /// </summary>
    public bool CpfRequired { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether CNPJ is required for vendors
    /// </summary>
    public bool CnpjRequired { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show CPF field on checkout
    /// </summary>
    public bool ShowCpfOnCheckout { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to show CPF field on customer info page
    /// </summary>
    public bool ShowCpfOnCustomerInfo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to show CNPJ field on vendor edit page
    /// </summary>
    public bool ShowCnpjOnVendorEdit { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to allow alphanumeric CNPJ
    /// </summary>
    public bool AllowAlphaNumericCnpj { get; set; } = true;
}

