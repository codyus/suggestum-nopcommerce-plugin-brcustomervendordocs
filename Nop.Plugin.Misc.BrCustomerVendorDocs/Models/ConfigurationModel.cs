using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Models;

/// <summary>
/// Represents configuration model
/// </summary>
public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.CpfRequired")]
    public bool CpfRequired { get; set; }

    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.CnpjRequired")]
    public bool CnpjRequired { get; set; }

    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCpfOnCheckout")]
    public bool ShowCpfOnCheckout { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCpfOnCustomerInfo")]
    public bool ShowCpfOnCustomerInfo { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCnpjOnVendorEdit")]
    public bool ShowCnpjOnVendorEdit { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.AllowAlphaNumericCnpj")]
    public bool AllowAlphaNumericCnpj { get; set; } = true;
}

