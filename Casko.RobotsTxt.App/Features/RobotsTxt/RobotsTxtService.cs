using Casko.AspNetCore.RobotsTxt;
using Casko.AspNetCore.RobotsTxt.Builders;
using Casko.AspNetCore.RobotsTxt.Validation;

namespace Casko.RobotsTxt.App.Features.RobotsTxt;

public class RobotsTxtService : IRobotsTxt
{
    public string GetRobotsTxt(HttpContext httpContext)
    {
        var robotsTxt =  new RobotsTxtBuilder(httpContext)
            .AddSitemap("/sitemap.xml")
            .AddUserAgent("*")
            .AddDisallow("/404")
            .AddDisallow("/private")
            .AddAllow("/private/login")
            .Build();

        var robotsTxtValidator = new RobotsTxtValidator(robotsTxt);

        if (!robotsTxtValidator.IsValid) throw new InvalidDataException(nameof(robotsTxt));
        
        return robotsTxt;
    }
}
