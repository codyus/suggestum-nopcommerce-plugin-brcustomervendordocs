namespace Nop.Plugin.Misc.BrCustomerVendorDocs;

/// <summary>
/// Represents plugin constants
/// </summary>
public class BrCustomerVendorDocsDefaults
{
    /// <summary>
    /// Gets a plugin system name
    /// </summary>
    public static string SystemName => "Misc.BrCustomerVendorDocs";

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    public static string CacheKey => "Nop.plugin.misc.brcustomervendordocs";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string CachePrefix => "Nop.plugin.misc.brcustomervendordocs";

    #region Routes

    public static class Routes
    {
        private const string ROUTE_PREFIX = "Plugin.Misc.BrCustomerVendorDocs.Route.";

        public static class Admin
        {
            public static string ConfigurationRouteName => ROUTE_PREFIX + "Configure";
        }

        public static class Public
        {
            public static string SaveCpfRouteName => ROUTE_PREFIX + "SaveCpf";
        }
    }

    #endregion

    #region Permissions

    public static class Permissions
    {
        public const string CONFIGURE = "BrCustomerVendorDocs.Configure";
    }

    #endregion
}

