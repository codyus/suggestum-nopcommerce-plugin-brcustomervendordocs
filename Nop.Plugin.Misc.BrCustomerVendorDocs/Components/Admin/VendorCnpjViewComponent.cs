using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Models;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;
using AdminWidgetZones = Nop.Web.Framework.Infrastructure.AdminWidgetZones;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Components.Admin;

/// <summary>
/// Represents a view component for displaying CNPJ field on vendor edit page
/// </summary>
public class VendorCnpjViewComponent : NopViewComponent
{
    #region Fields

    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly BrVendorDocumentService _vendorDocumentService;

    #endregion

    #region Ctor

    public VendorCnpjViewComponent(
        BrCustomerVendorDocsSettings settings,
        BrVendorDocumentService vendorDocumentService)
    {
        _settings = settings;
        _vendorDocumentService = vendorDocumentService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoke the view component
    /// </summary>
    /// <param name="widgetZone">Widget zone name</param>
    /// <param name="additionalData">Additional data</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view component result
    /// </returns>
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        if (widgetZone != AdminWidgetZones.VendorDetailsBlock)
            return Content("");

        if (!_settings.ShowCnpjOnVendorEdit)
            return Content("");

        var vendorModel = additionalData as VendorModel;
        if (vendorModel == null)
            return Content("");

        string cnpj = string.Empty;
        if (vendorModel.Id > 0)
        {
            var document = await _vendorDocumentService.GetByVendorIdAsync(vendorModel.Id);
            cnpj = document?.Cnpj ?? string.Empty;
        }

        var model = new VendorCnpjModel
        {
            Cnpj = cnpj,
            Required = _settings.CnpjRequired,
            AllowAlphaNumeric = _settings.AllowAlphaNumericCnpj
        };

        return View("~/Plugins/Misc.BrCustomerVendorDocs/Components/Admin/VendorCnpj.cshtml", model);
    }

    #endregion
}

