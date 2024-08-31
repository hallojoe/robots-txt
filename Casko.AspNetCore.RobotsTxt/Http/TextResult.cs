using System.Text;
using Microsoft.AspNetCore.Http;

namespace Casko.AspNetCore.RobotsTxt.Http;

public class TextResult(string? result) : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var serializedResult = result ?? string.Empty;

        httpContext.Response.ContentType = Constants.MimeType;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(serializedResult);

        await httpContext.Response.WriteAsync(serializedResult, Encoding.UTF8);
    }
}