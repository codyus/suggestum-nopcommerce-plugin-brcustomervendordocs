using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure.EventConsumers;

/// <summary>
/// Event consumer to validate CPF when customer model is received in admin
/// </summary>
public class CustomerModelReceivedEventConsumer : IConsumer<ModelReceivedEvent<BaseNopModel>>
{
    #region Fields

    private readonly BrCustomerDocumentService _customerDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly ILocalizationService _localizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public CustomerModelReceivedEventConsumer(
        BrCustomerDocumentService customerDocumentService,
        BrCustomerVendorDocsSettings settings,
        ILocalizationService localizationService,
        IHttpContextAccessor httpContextAccessor,
        INotificationService notificationService)
    {
        _customerDocumentService = customerDocumentService;
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
        // Only process CustomerModel
        if (eventMessage.Model is not CustomerModel model)
            return;

        var modelState = eventMessage.ModelState;

        if (model.Id == 0)
            return;

        // Get CPF from form (it's not in the model, so we need to get it from form)
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        var (keyExists, cpfValue) = await httpContext.Request.TryGetFormValueAsync("BrCustomerCpf");
        if (!keyExists || StringValues.IsNullOrEmpty(cpfValue))
        {
            // Validate required if CPF is required
            if (_settings.CpfRequired)
            {
                var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Required");
                modelState.AddModelError("BrCustomerCpf", errorMessage);
                _notificationService.ErrorNotification(errorMessage);
            }
            return;
        }

        var cpf = cpfValue.ToString().Trim();

        // Validate required
        if (string.IsNullOrWhiteSpace(cpf))
        {
            if (_settings.CpfRequired)
            {
                var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Required");
                modelState.AddModelError("BrCustomerCpf", errorMessage);
                _notificationService.ErrorNotification(errorMessage);
            }
            return;
        }

        // Validate format
        var normalized = CpfValidator.Normalize(cpf);
        if (!CpfValidator.IsValid(normalized))
        {
            var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Invalid");
            modelState.AddModelError("BrCustomerCpf", errorMessage);
            _notificationService.ErrorNotification(errorMessage);
            return;
        }

        // Check for duplicate
        var existingDocument = await _customerDocumentService.GetByCustomerIdAsync(model.Id);
        if (existingDocument == null || existingDocument.Cpf != normalized)
        {
            if (await _customerDocumentService.CpfExistsAsync(normalized, model.Id))
            {
                var errorMessage = await _localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Duplicate");
                modelState.AddModelError("BrCustomerCpf", errorMessage);
                _notificationService.ErrorNotification(errorMessage);
            }
        }
    }

    #endregion
}

