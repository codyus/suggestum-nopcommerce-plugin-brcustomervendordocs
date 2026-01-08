using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Controllers.Admin;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class BrCustomerVendorDocsAdminController : BasePluginController
{
    #region Fields

    private readonly BrCustomerVendorDocsSettings _settings;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public BrCustomerVendorDocsAdminController(
        BrCustomerVendorDocsSettings settings,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ISettingService settingService)
    {
        _settings = settings;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public IActionResult Configure()
    {
        var model = new ConfigurationModel
        {
            CpfRequired = _settings.CpfRequired,
            CnpjRequired = _settings.CnpjRequired,
            ShowCpfOnCheckout = _settings.ShowCpfOnCheckout,
            ShowCpfOnCustomerInfo = _settings.ShowCpfOnCustomerInfo,
            ShowCnpjOnVendorEdit = _settings.ShowCnpjOnVendorEdit,
            AllowAlphaNumericCnpj = _settings.AllowAlphaNumericCnpj
        };

        return View("~/Plugins/Misc.BrCustomerVendorDocs/Views/Admin/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        _settings.CpfRequired = model.CpfRequired;
        _settings.CnpjRequired = model.CnpjRequired;
        _settings.ShowCpfOnCheckout = model.ShowCpfOnCheckout;
        _settings.ShowCpfOnCustomerInfo = model.ShowCpfOnCustomerInfo;
        _settings.ShowCnpjOnVendorEdit = model.ShowCnpjOnVendorEdit;
        _settings.AllowAlphaNumericCnpj = model.AllowAlphaNumericCnpj;

        await _settingService.SaveSettingAsync(_settings);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return Configure();
    }

    #endregion
}

