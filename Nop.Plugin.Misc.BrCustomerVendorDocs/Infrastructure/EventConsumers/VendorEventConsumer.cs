using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Nop.Core.Domain.Vendors;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;
using Nop.Services.Events;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure.EventConsumers;

/// <summary>
/// Represents plugin event consumer for vendor
/// </summary>
public class VendorEventConsumer : IConsumer<EntityInsertedEvent<Vendor>>,
    IConsumer<EntityUpdatedEvent<Vendor>>
{
    #region Fields

    private readonly BrVendorDocumentService _vendorDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Ctor

    public VendorEventConsumer(
        BrVendorDocumentService vendorDocumentService,
        BrCustomerVendorDocsSettings settings,
        IHttpContextAccessor httpContextAccessor)
    {
        _vendorDocumentService = vendorDocumentService;
        _settings = settings;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle vendor inserted event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage)
    {
        await SaveVendorCnpjAsync(eventMessage.Entity);
    }

    /// <summary>
    /// Handle vendor updated event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Vendor> eventMessage)
    {
        await SaveVendorCnpjAsync(eventMessage.Entity);
    }

    /// <summary>
    /// Save vendor CNPJ from form
    /// </summary>
    /// <param name="vendor">Vendor</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    private async Task SaveVendorCnpjAsync(Vendor vendor)
    {
        if (vendor == null)
            return;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        // Only process if this is from admin area
        var path = httpContext.Request.Path.Value ?? "";
        if (!path.Contains("/Admin/Vendor/Edit", StringComparison.InvariantCultureIgnoreCase))
            return;

        var (keyExists, cnpjValue) = await httpContext.Request.TryGetFormValueAsync("BrVendorCnpj");
        if (!keyExists || StringValues.IsNullOrEmpty(cnpjValue))
        {
            // Delete CNPJ if empty (validation is done in ModelReceivedEvent)
            if (!_settings.CnpjRequired)
            {
                await _vendorDocumentService.DeleteAsync(vendor.Id);
            }
            return;
        }

        var cnpj = cnpjValue.ToString().Trim();
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            // Delete CNPJ if empty (validation is done in ModelReceivedEvent)
            if (!_settings.CnpjRequired)
            {
                await _vendorDocumentService.DeleteAsync(vendor.Id);
            }
            return;
        }

        // Validate CNPJ before saving (double check even though ModelReceivedEvent should have validated)
        var normalized = CnpjValidator.Normalize(cnpj);
        if (!CnpjValidator.IsValid(normalized, _settings.AllowAlphaNumericCnpj))
        {
            // Don't save invalid CNPJ - validation error should have been added in ModelReceivedEvent
            return;
        }

        // Check for duplicate CNPJ
        var existingDocument = await _vendorDocumentService.GetByVendorIdAsync(vendor.Id);
        if (existingDocument == null || existingDocument.Cnpj != normalized)
        {
            if (await _vendorDocumentService.CnpjExistsAsync(normalized, vendor.Id))
            {
                // Don't save duplicate CNPJ - validation error should have been added in ModelReceivedEvent
                return;
            }
        }

        try
        {
            // Save CNPJ
            await _vendorDocumentService.SaveAsync(vendor.Id, normalized, _settings.AllowAlphaNumericCnpj);
        }
        catch (ArgumentException)
        {
            // Invalid CNPJ - validation error already added in ModelReceivedEvent
        }
    }

    #endregion
}

