using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;
using Nop.Services.Events;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure.EventConsumers;

/// <summary>
/// Event consumer to save customer CPF when customer is updated in admin
/// </summary>
public class CustomerEventConsumer : IConsumer<EntityUpdatedEvent<Customer>>
{
    #region Fields

    private readonly BrCustomerDocumentService _customerDocumentService;
    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Ctor

    public CustomerEventConsumer(
        BrCustomerDocumentService customerDocumentService,
        BrCustomerVendorDocsSettings settings,
        IHttpContextAccessor httpContextAccessor)
    {
        _customerDocumentService = customerDocumentService;
        _settings = settings;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle customer updated event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Customer> eventMessage)
    {
        var customer = eventMessage.Entity;
        if (customer == null)
            return;

        // Only process if this is from admin area
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        // Check if this is from admin customer edit page
        var path = httpContext.Request.Path.Value ?? "";
        if (!path.Contains("/Admin/Customer/Edit", StringComparison.InvariantCultureIgnoreCase))
            return;

        // Get CPF from form
        var (keyExists, cpfValue) = await httpContext.Request.TryGetFormValueAsync("BrCustomerCpf");
        if (!keyExists || StringValues.IsNullOrEmpty(cpfValue))
        {
            // Delete CPF if empty
            await _customerDocumentService.DeleteAsync(customer.Id);
            return;
        }

        var cpf = cpfValue.ToString().Trim();
        if (string.IsNullOrWhiteSpace(cpf))
        {
            // Delete CPF if empty (validation is done in ModelReceivedEvent)
            if (!_settings.CpfRequired)
            {
                await _customerDocumentService.DeleteAsync(customer.Id);
            }
            return;
        }

        // Validate CPF before saving (double check even though ModelReceivedEvent should have validated)
        var normalized = CpfValidator.Normalize(cpf);
        if (!CpfValidator.IsValid(normalized))
        {
            // Don't save invalid CPF - validation error should have been added in ModelReceivedEvent
            return;
        }

        // Check for duplicate CPF
        var existingDocument = await _customerDocumentService.GetByCustomerIdAsync(customer.Id);
        if (existingDocument == null || existingDocument.Cpf != normalized)
        {
            if (await _customerDocumentService.CpfExistsAsync(normalized, customer.Id))
            {
                // Don't save duplicate CPF - validation error should have been added in ModelReceivedEvent
                return;
            }
        }

        try
        {
            // Save CPF
            await _customerDocumentService.SaveAsync(customer.Id, normalized);
        }
        catch (ArgumentException)
        {
            // Invalid CPF - validation error already added in ModelReceivedEvent
        }
    }

    #endregion
}

