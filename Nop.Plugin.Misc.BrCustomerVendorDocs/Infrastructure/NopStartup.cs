using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure.EventConsumers;
using Nop.Plugin.Misc.BrCustomerVendorDocs.Services;

namespace Nop.Plugin.Misc.BrCustomerVendorDocs.Infrastructure;

/// <summary>
/// Represents object for the configuring services on application startup
/// </summary>
public class NopStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewLocationExpander());
        });

        //register custom services
        services.AddScoped<BrCustomerDocumentService>();
        services.AddScoped<BrVendorDocumentService>();
        services.AddScoped<BrCustomerVendorDocsPermissionConfigManager>();
        services.AddScoped<VendorEventConsumer>();
        services.AddScoped<CustomerEventConsumer>();
        services.AddScoped<CustomerModelReceivedEventConsumer>();
        services.AddScoped<VendorModelReceivedEventConsumer>();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 3000;
}

