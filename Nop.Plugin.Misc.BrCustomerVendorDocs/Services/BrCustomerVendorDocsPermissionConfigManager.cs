using Nop.Core.Domain.Customers;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Services;

/// <summary>
/// Brazilian Customer and Vendor Documents permission configuration manager
/// </summary>
public class BrCustomerVendorDocsPermissionConfigManager : IPermissionConfigManager
{
    /// <summary>
    /// Gets all permission configurations
    /// </summary>
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new("Admin area. Configure Brazilian Customer and Vendor Documents plugin",
            BrCustomerVendorDocsDefaults.Permissions.CONFIGURE,
            nameof(StandardPermission.Configuration),
            NopCustomerDefaults.AdministratorsRoleName)
    };
}

