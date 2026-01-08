using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Models;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;
using AdminWidgetZones = Nop.Web.Framework.Infrastructure.AdminWidgetZones;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Components.Admin;

/// <summary>
/// Represents a view component for displaying CPF field on customer edit page in admin
/// </summary>
public class CustomerCpfViewComponent : NopViewComponent
{
    #region Fields

    private readonly BrCustomerDocumentService _customerDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;

    #endregion

    #region Ctor

    public CustomerCpfViewComponent(
        BrCustomerDocumentService customerDocumentService,
        BrCustomerVendorDocsSettings settings)
    {
        _customerDocumentService = customerDocumentService;
        _settings = settings;
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
        if (widgetZone != AdminWidgetZones.CustomerDetailsBlock)
            return Content("");

        if (additionalData is not CustomerModel customerModel || customerModel.Id == 0)
            return Content("");

        var document = await _customerDocumentService.GetByCustomerIdAsync(customerModel.Id);
        var cpf = document?.Cpf ?? string.Empty;

        var model = new CustomerCpfModel
        {
            Cpf = cpf,
            Required = _settings.CpfRequired,
            CustomerId = customerModel.Id
        };

        return View("~/Plugins/Misc.BrCustomerVendorDocs/Components/Admin/CustomerCpf.cshtml", model);
    }

    #endregion
}

