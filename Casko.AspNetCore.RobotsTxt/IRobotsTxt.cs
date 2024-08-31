using Microsoft.AspNetCore.Http;

namespace Casko.AspNetCore.RobotsTxt;

public interface IRobotsTxt
{
    string GetRobotsTxt(HttpContext httpContext);
}