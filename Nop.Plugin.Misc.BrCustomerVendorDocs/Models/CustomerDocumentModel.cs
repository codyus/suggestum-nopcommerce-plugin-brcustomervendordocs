using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Models;

/// <summary>
/// Represents customer document model
/// </summary>
public record CustomerDocumentModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf")]
    public string Cpf { get; set; }
}

