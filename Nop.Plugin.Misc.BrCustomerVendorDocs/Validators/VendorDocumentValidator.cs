using FluentValidation;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Models;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services.Validators;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Validators;

/// <summary>
/// Vendor document validator
/// </summary>
public class VendorDocumentValidator : BaseNopValidator<VendorDocumentModel>
{
    public VendorDocumentValidator(
        ILocalizationService localizationService,
        BrVendorDocumentService vendorDocumentService,
        BrCustomerVendorDocsSettings settings)
    {
        if (settings.CnpjRequired)
        {
            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Required"));
        }

        RuleFor(x => x.Cnpj)
            .Must((model, cnpj) =>
            {
                if (string.IsNullOrWhiteSpace(cnpj))
                    return true; // Optional field

                var normalized = CnpjValidator.Normalize(cnpj);
                if (!CnpjValidator.IsValid(normalized, settings.AllowAlphaNumericCnpj))
                    return false;

                // Note: VendorId would need to be passed in the model or context
                // For now, we'll skip duplicate check in validator and handle it in controller
                return true;
            })
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Invalid"));
    }
}

