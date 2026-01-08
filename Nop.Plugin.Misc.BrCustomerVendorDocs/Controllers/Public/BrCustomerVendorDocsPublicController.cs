using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Controllers;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Controllers.Public;

[AutoValidateAntiforgeryToken]
public class BrCustomerVendorDocsPublicController : BasePublicController
{
    #region Fields

    private readonly BrCustomerDocumentService _customerDocumentService;
    private readonly ICustomerService _customerService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public BrCustomerVendorDocsPublicController(
        BrCustomerDocumentService customerDocumentService,
        ICustomerService customerService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IWorkContext workContext)
    {
        _customerDocumentService = customerDocumentService;
        _customerService = customerService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    [HttpPost]
    public async Task<IActionResult> SaveCpf(string cpf)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();

        if (customer == null || await _customerService.IsGuestAsync(customer))
        {
            return Json(new { success = false, message = await _localizationService.GetResourceAsync("Account.Login") });
        }

        try
        {
            // Check if CPF already exists for another customer
            if (!string.IsNullOrWhiteSpace(cpf) && await _customerDocumentService.CpfExistsAsync(cpf, customer.Id))
            {
                return Json(new
                {
                    success = false,
                    message = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Duplicate")
                });
            }

            if (string.IsNullOrWhiteSpace(cpf))
            {
                await _customerDocumentService.DeleteAsync(customer.Id);
            }
            else
            {
                await _customerDocumentService.SaveAsync(customer.Id, cpf);
            }

            return Json(new { success = true });
        }
        catch (ArgumentException)
        {
            return Json(new
            {
                success = false,
                message = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Invalid")
            });
        }
    }

    #endregion
}

