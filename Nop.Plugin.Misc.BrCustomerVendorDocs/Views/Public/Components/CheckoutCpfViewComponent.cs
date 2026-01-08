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
/// Represents a view component for displaying CPF field on checkout billing address
/// </summary>
public class CheckoutCpfViewComponent : NopViewComponent
{
    #region Fields

    private readonly BrCustomerDocumentService _customerDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly ICustomerService _customerService;
    private readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public CheckoutCpfViewComponent(
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
        if (widgetZone != PublicWidgetZones.CheckoutBillingAddressMiddle)
            return Content("");

        if (!_settings.ShowCpfOnCheckout)
            return Content("");

        var customer = await _workContext.GetCurrentCustomerAsync();
        string cpf = string.Empty;

        if (customer != null && !await _customerService.IsGuestAsync(customer))
        {
            var document = await _customerDocumentService.GetByCustomerIdAsync(customer.Id);
            cpf = document?.Cpf ?? string.Empty;
        }

        return View("~/Plugins/Misc.BrCustomerVendorDocs/Views/Public/Components/CheckoutCpf.cshtml", new { Cpf = cpf, Required = _settings.CpfRequired });
    }

    #endregion
}

