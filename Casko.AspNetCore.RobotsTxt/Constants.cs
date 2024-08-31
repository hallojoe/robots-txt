namespace Casko.AspNetCore.RobotsTxt;

internal static class Constants
{
    internal const string MimeType = "text/plain";
    internal const string FileName = "robots.txt";
    internal const string RouteBase = "api/robots-txt";
    internal const string Sitemap = "Sitemap";
    internal const string UserAgent = "User-agent";
    internal const string Allow = "Allow";
    internal const string Disallow = "Disallow";
    internal const string SchemeSeparator = "://";

    internal const string OnlyRelativeUrlsAreAllowed = "Only relative URLs are allowed.";
    internal const string CannotAddRelativeUrl = "Cannot add relative sitemap URL without HttpContext.";
}