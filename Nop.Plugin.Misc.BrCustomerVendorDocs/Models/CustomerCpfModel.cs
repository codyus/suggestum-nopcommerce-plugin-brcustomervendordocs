namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Models;

/// <summary>
/// Model for customer CPF in admin
/// </summary>
public class CustomerCpfModel
{
    /// <summary>
    /// Gets or sets the CPF
    /// </summary>
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether CPF is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public int CustomerId { get; set; }
}

