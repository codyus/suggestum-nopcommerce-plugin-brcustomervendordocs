using Nop.Core;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;

/// <summary>
/// Represents a Brazilian vendor document (CNPJ)
/// </summary>
public class BrVendorDocument : BaseEntity
{
    /// <summary>
    /// Gets or sets the vendor identifier
    /// </summary>
    public int VendorId { get; set; }

    /// <summary>
    /// Gets or sets the CNPJ (normalized, uppercase for alphanumeric)
    /// </summary>
    public string Cnpj { get; set; }

    /// <summary>
    /// Gets or sets the date and time of entity creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time of entity update
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}

