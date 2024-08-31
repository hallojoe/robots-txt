using System.Text.RegularExpressions;
using Casko.AspNetCore.RobotsTxt.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;

namespace Casko.AspNetCore.RobotsTxt.Extensions;

public static class ConfigurationExtensions
{
    private static void AddXmlRobotsTxtServiceType(this IServiceCollection services)
    {
        var robotsTxtServiceType = AppDomain.CurrentDomain.GetRobotsTxtServiceType();
        
        if (robotsTxtServiceType is not null) services.AddTransient(typeof(IRobotsTxt), robotsTxtServiceType);
    }

    private static string CreateEndPointRoute()
    {
        var fileNameWithoutExtension = Constants.FileName[..Constants.FileName.LastIndexOf('.')];

        var result = $"{Constants.RouteBase}/{fileNameWithoutExtension}";

        return result;
    }

    private static void CreateEndPoint(this IApplicationBuilder applicationBuilder)
    {
        var robotsTxtService = applicationBuilder.ApplicationServices.GetRequiredService<IRobotsTxt>();

        applicationBuilder.UseEndpoints(endPoints =>
        {
            var route = CreateEndPointRoute();
            
            endPoints.MapGet(route, (HttpContext httpContext) => new TextResult(robotsTxtService.GetRobotsTxt(httpContext)));
        });
    }

    private static void CreateRewrite(this IApplicationBuilder applicationBuilder)
    {
        var route = CreateEndPointRoute();

        var rewriteOptions = new RewriteOptions();
        
        var pattern = $"^{Regex.Escape(Constants.FileName)}";

        rewriteOptions.AddRewrite(pattern, route, true);

        applicationBuilder.UseRewriter(rewriteOptions);
    }

    public static IServiceCollection AddRobotsTxt(this IServiceCollection? services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        services.AddXmlRobotsTxtServiceType();

        return services;
    }

    public static IApplicationBuilder UseRobotsTxt(this IApplicationBuilder applicationBuilder, bool useRewrites = false)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);
        
        applicationBuilder.CreateEndPoint();

        if (useRewrites) applicationBuilder.UseRobotsTxtRewrite();
        
        return applicationBuilder;
    }

    public static IApplicationBuilder UseRobotsTxtRewrite(this IApplicationBuilder? applicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);

        applicationBuilder.CreateRewrite();

        return applicationBuilder;
    }
    
}