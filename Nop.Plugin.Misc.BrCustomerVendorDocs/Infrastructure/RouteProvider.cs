using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure;

/// <summary>
/// Represents plugin route provider
/// </summary>
public class RouteProvider : BaseRouteProvider, IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
            name: BrCustomerVendorDocsDefaults.Routes.Admin.ConfigurationRouteName,
            pattern: "Admin/BrCustomerVendorDocs/Configure",
            defaults: new { controller = "BrCustomerVendorDocsAdmin", action = "Configure", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(
            name: BrCustomerVendorDocsDefaults.Routes.Public.SaveCpfRouteName,
            pattern: "BrCustomerVendorDocs/SaveCpf",
            defaults: new { controller = "BrCustomerVendorDocsPublic", action = "SaveCpf" });
    }

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}

