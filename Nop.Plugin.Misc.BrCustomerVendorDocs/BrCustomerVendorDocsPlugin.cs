using Nop.Core.Domain.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;
using AdminWidgetZones = Nop.Web.Framework.Infrastructure.AdminWidgetZones;
using PublicWidgetZones = Nop.Web.Framework.Infrastructure.PublicWidgetZones;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs;

/// <summary>
/// Represents the Brazilian Customer and Vendor Documents plugin
/// </summary>
public class BrCustomerVendorDocsPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;
    private readonly INopUrlHelper _nopUrlHelper;
    private readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public BrCustomerVendorDocsPlugin(ILocalizationService localizationService,
        ISettingService settingService,
        INopUrlHelper nopUrlHelper,
        WidgetSettings widgetSettings)
    {
        _localizationService = localizationService;
        _settingService = settingService;
        _nopUrlHelper = nopUrlHelper;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return _nopUrlHelper.RouteUrl(BrCustomerVendorDocsDefaults.Routes.Admin.ConfigurationRouteName);
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        //settings
        var settings = new BrCustomerVendorDocsSettings
        {
            CpfRequired = false,
            CnpjRequired = false,
            ShowCpfOnCheckout = true,
            ShowCpfOnCustomerInfo = true,
            ShowCnpjOnVendorEdit = true,
            AllowAlphaNumericCnpj = true
        };
        await _settingService.SaveSettingAsync(settings);

        //locales
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            // Plugin
            ["Plugins.Misc.BrCustomerVendorDocs.FriendlyName"] = "Brazilian Customer and Vendor Documents",
            ["Plugins.Misc.BrCustomerVendorDocs.Description"] = "This plugin adds CPF (Customer) and CNPJ (Vendor) fields with Brazilian document validation",

            // Configuration
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration"] = "Configuration",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.CpfRequired"] = "CPF Required",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.CpfRequired.Hint"] = "Check to make CPF required for customers.",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.CnpjRequired"] = "CNPJ Required",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.CnpjRequired.Hint"] = "Check to make CNPJ required for vendors.",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCpfOnCheckout"] = "Show CPF on Checkout",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCpfOnCheckout.Hint"] = "Check to show CPF field on checkout billing address.",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCpfOnCustomerInfo"] = "Show CPF on Customer Info",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCpfOnCustomerInfo.Hint"] = "Check to show CPF field on customer info page.",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCnpjOnVendorEdit"] = "Show CNPJ on Vendor Edit",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.ShowCnpjOnVendorEdit.Hint"] = "Check to show CNPJ field on vendor edit page.",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.AllowAlphaNumericCnpj"] = "Allow Alphanumeric CNPJ",
            ["Plugins.Misc.BrCustomerVendorDocs.Configuration.Fields.AllowAlphaNumericCnpj.Hint"] = "Check to allow alphanumeric CNPJ format (14 characters A-Z0-9).",

            // Fields
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf"] = "CPF",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Hint"] = "Enter your CPF (Brazilian individual taxpayer registry).",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj"] = "CNPJ",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Hint"] = "Enter your CNPJ (Brazilian corporate taxpayer registry).",

            // Validation errors
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Required"] = "CPF is required.",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Invalid"] = "CPF is invalid.",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cpf.Duplicate"] = "This CPF is already registered.",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Required"] = "CNPJ is required.",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Invalid"] = "CNPJ is invalid.",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.Duplicate"] = "This CNPJ is already registered.",
            ["Plugins.Misc.BrCustomerVendorDocs.Fields.Cnpj.InvalidFormat"] = "CNPJ format is invalid. Use 14 digits or 14 alphanumeric characters.",
        });

        //widget
        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(BrCustomerVendorDocsDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(BrCustomerVendorDocsDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        //settings
        await _settingService.DeleteSettingAsync<BrCustomerVendorDocsSettings>();

        //widget
        if (_widgetSettings.ActiveWidgetSystemNames.Contains(BrCustomerVendorDocsDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(BrCustomerVendorDocsDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.BrCustomerVendorDocs");
    }

    #region IWidgetPlugin

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone == PublicWidgetZones.CustomerInfoBottom)
            return typeof(Views.Public.Components.CustomerCpfViewComponent);

        if (widgetZone == PublicWidgetZones.CheckoutBillingAddressMiddle)
            return typeof(Views.Public.Components.CheckoutCpfViewComponent);

        return null;
    }

    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the widget zones
    /// </returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>([
            PublicWidgetZones.CustomerInfoBottom,
            PublicWidgetZones.CheckoutBillingAddressMiddle
        ]);
    }

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => true;

    #endregion

    #endregion
}

