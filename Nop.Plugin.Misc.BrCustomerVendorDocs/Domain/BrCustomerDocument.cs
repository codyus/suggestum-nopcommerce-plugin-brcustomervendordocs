using Nop.Core;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Domain;

/// <summary>
/// Represents a Brazilian customer document (CPF)
/// </summary>
public class BrCustomerDocument : BaseEntity
{
    /// <summary>
    /// Gets or sets the customer identifier
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the CPF (normalized, digits only)
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    /// Gets or sets the date and time of entity creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time of entity update
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}

