using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Services.Customers;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;
using PublicWidgetZones = Nop.Web.Framework.Infrastructure.PublicWidgetZones;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Views.Public.Components;

/// <summary>
/// Represents a view component for displaying CPF field on customer info page
/// </summary>
public class CustomerCpfViewComponent : NopViewComponent
{
    #region Fields

    private readonly BrCustomerDocumentService _customerDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly ICustomerService _customerService;
    private readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public CustomerCpfViewComponent(
        BrCustomerDocumentService customerDocumentService,
        BrCustomerVendorDocsSettings settings,
        ICustomerService customerService,
        IWorkContext workContext)
    {
        _customerDocumentService = customerDocumentService;
        _settings = settings;
        _customerService = customerService;
        _workContext = workContext;
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
        if (widgetZone != PublicWidgetZones.CustomerInfoBottom)
            return Content("");

        if (!_settings.ShowCpfOnCustomerInfo)
            return Content("");

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == null || await _customerService.IsGuestAsync(customer))
            return Content("");

        var document = await _customerDocumentService.GetByCustomerIdAsync(customer.Id);
        var cpf = document?.Cpf ?? string.Empty;

        return View("~/Plugins/Misc.BrCustomerVendorDocs/Views/Public/Components/CustomerCpf.cshtml", new { Cpf = cpf, Required = _settings.CpfRequired });
    }

    #endregion
}

