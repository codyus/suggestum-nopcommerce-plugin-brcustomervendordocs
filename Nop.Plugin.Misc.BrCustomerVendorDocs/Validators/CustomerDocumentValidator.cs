using FluentValidation;
using Nop.Core;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Models;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Validators;

/// <summary>
/// Customer document validator
/// </summary>
public class CustomerDocumentValidator : BaseNopValidator<CustomerDocumentModel>
{
    public CustomerDocumentValidator(
        ILocalizationService localizationService,
        BrCustomerDocumentService customerDocumentService,
        BrCustomerVendorDocsSettings settings,
        IWorkContext workContext)
    {
        if (settings.CpfRequired)
        {
            RuleFor(x => x.Cpf)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Required"));
        }

        RuleFor(x => x.Cpf)
            .MustAsync(async (model, cpf, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(cpf))
                    return true; // Optional field

                var normalized = CpfValidator.Normalize(cpf);
                if (!CpfValidator.IsValid(normalized))
                    return false;

                var customer = await workContext.GetCurrentCustomerAsync();
                // Check if CPF already exists for another customer
                if (customer != null && await customerDocumentService.CpfExistsAsync(normalized, customer.Id))
                    return false;

                return true;
            })
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Invalid"));
    }
}

