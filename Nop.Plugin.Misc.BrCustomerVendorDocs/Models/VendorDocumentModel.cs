using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Models;

/// <summary>
/// Represents vendor document model
/// </summary>
public record VendorDocumentModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj")]
    public string Cnpj { get; set; }
}

