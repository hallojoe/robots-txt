using Casko.AspNetCore.RobotsTxt;
using Casko.AspNetCore.RobotsTxt.Builders;
using Casko.AspNetCore.RobotsTxt.Validation;

namespace Casko.RobotsTxt.App.Features.RobotsTxt;

public class RobotsTxtService : IRobotsTxt
{
    // Implement
    public string GetRobotsTxt(HttpContext httpContext)
    {
        // Create
        var robotsTxt =  new RobotsTxtBuilder(httpContext)
            // When no url is passed then base url will be created from current request and filename will be sitemap.xml.
            // When relative url is passed then base url will be created from current request.
            .AddSitemap("/sitemap.xml") 
            .AddUserAgent("*")
            .AddDisallow("/404")
            .AddDisallow("/private")
            .AddAllow("/private/login")
            .Build();

        // Validate
        var robotsTxtValidator = new RobotsTxtValidator(robotsTxt);

        if (!robotsTxtValidator.IsValid) throw new InvalidDataException(nameof(robotsTxt));
        
        return robotsTxt;
    }
}
