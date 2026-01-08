using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure.EventConsumers;

/// <summary>
/// Event consumer to validate CNPJ when vendor model is received in admin
/// </summary>
public class VendorModelReceivedEventConsumer : IConsumer<ModelReceivedEvent<BaseNopModel>>
{
    #region Fields

    private readonly BrVendorDocumentService _vendorDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly ILocalizationService _localizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public VendorModelReceivedEventConsumer(
        BrVendorDocumentService vendorDocumentService,
        BrCustomerVendorDocsSettings settings,
        ILocalizationService localizationService,
        IHttpContextAccessor httpContextAccessor,
        INotificationService notificationService)
    {
        _vendorDocumentService = vendorDocumentService;
        _settings = settings;
        _localizationService = localizationService;
        _httpContextAccessor = httpContextAccessor;
        _notificationService = notificationService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle model received event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(ModelReceivedEvent<BaseNopModel> eventMessage)
    {
        // Only process VendorModel
        if (eventMessage.Model is not VendorModel model)
            return;

        var modelState = eventMessage.ModelState;

        if (model.Id == 0)
            return;

        // Get CNPJ from form (it's not in the model, so we need to get it from form)
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        var (keyExists, cnpjValue) = await httpContext.Request.TryGetFormValueAsync("BrVendorCnpj");
        if (!keyExists || StringValues.IsNullOrEmpty(cnpjValue))
        {
            // Validate required if CNPJ is required
            if (_settings.CnpjRequired)
            {
                var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Required");
                modelState.AddModelError("BrVendorCnpj", errorMessage);
                _notificationService.ErrorNotification(errorMessage);
            }
            return;
        }

        var cnpj = cnpjValue.ToString().Trim();

        // Validate required
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            if (_settings.CnpjRequired)
            {
                var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Required");
                modelState.AddModelError("BrVendorCnpj", errorMessage);
                _notificationService.ErrorNotification(errorMessage);
            }
            return;
        }

        // Validate format
        var normalized = CnpjValidator.Normalize(cnpj);
        if (!CnpjValidator.IsValid(normalized, _settings.AllowAlphaNumericCnpj))
        {
            var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Invalid");
            modelState.AddModelError("BrVendorCnpj", errorMessage);
            _notificationService.ErrorNotification(errorMessage);
            return;
        }

        // Check for duplicate
        var existingDocument = await _vendorDocumentService.GetByVendorIdAsync(model.Id);
        if (existingDocument == null || existingDocument.Cnpj != normalized)
        {
            if (await _vendorDocumentService.CnpjExistsAsync(normalized, model.Id))
            {
                var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Duplicate");
                modelState.AddModelError("BrVendorCnpj", errorMessage);
                _notificationService.ErrorNotification(errorMessage);
            }
        }
    }

    #endregion
}

