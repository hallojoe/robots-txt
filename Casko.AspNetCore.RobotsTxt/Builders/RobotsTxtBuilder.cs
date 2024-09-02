using System.Text;
using Microsoft.AspNetCore.Http;

namespace Casko.AspNetCore.RobotsTxt.Builders;

public class RobotsTxtBuilder
{
    private readonly StringBuilder _content = new();
    private readonly List<string> _userAgents = [];
    private readonly List<string> _disallows = [];
    private readonly List<string> _allows = [];
    private readonly List<string> _sitemaps = [];
    private readonly HttpContext? _httpContext;
        
    private static readonly char[] Separator = ['\r', '\n'];

    public RobotsTxtBuilder(HttpContext? httpContext = null)
    {
        _httpContext = httpContext;
         
    }
        
    public RobotsTxtBuilder(string robotsTxt, HttpContext? httpContext = null)
    {
        _httpContext = httpContext;
         
        ParseRobotsTxt(robotsTxt);
    }
        
    private void ParseRobotsTxt(string robotsTxt)
    {
        var lines = robotsTxt.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            if (line.StartsWith($"{Constants.UserAgent}:", StringComparison.OrdinalIgnoreCase))
            {
                _userAgents.Add(line[$"{Constants.UserAgent}:".Length..].Trim());
            }
            else if (line.StartsWith($"{Constants.Disallow}:", StringComparison.OrdinalIgnoreCase))
            {
                _disallows.Add(line[$"{Constants.Disallow}:".Length..].Trim());
            }
            else if (line.StartsWith($"{Constants.Allow}:", StringComparison.OrdinalIgnoreCase))
            {
                _allows.Add(line[$"{Constants.Allow}:".Length..].Trim());
            }
            else if (line.StartsWith($"{Constants.Sitemap}:", StringComparison.OrdinalIgnoreCase))
            {
                _sitemaps.Add(line[$"{Constants.Sitemap}:".Length..].Trim());
            }
        }
    }

    private string EnsureRelativeUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return url;
            
        if (_httpContext == null || !uri.IsAbsoluteUri) throw new ArgumentException(Constants.OnlyRelativeUrlsAreAllowed);
                
        var baseUri = new Uri($"{_httpContext.Request.Scheme}{Constants.SchemeSeparator}{_httpContext.Request.Host}/");

        return baseUri.MakeRelativeUri(uri).ToString();

    }

    public RobotsTxtBuilder AddUserAgent(string userAgent)
    {
        _userAgents.Add(userAgent);
        return this;
    }

    public RobotsTxtBuilder AddDisallow(string path)
    {
        _disallows.Add(EnsureRelativeUrl(path));
        return this;
    }

    public RobotsTxtBuilder AddAllow(string path)
    {
        _allows.Add(EnsureRelativeUrl(path));
        return this;
    }

    public RobotsTxtBuilder AddSitemap(string sitemapUrl = "/sitemap.xml")
    {
        if (!Uri.TryCreate(sitemapUrl, UriKind.Absolute, out _))
        {
            if (_httpContext == null)
            {
                throw new InvalidOperationException(Constants.CannotAddRelativeUrl);
            }

            var baseUrl = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}";

            sitemapUrl = new Uri(new Uri(baseUrl), sitemapUrl).ToString();
                
        }
            
        _sitemaps.Add(sitemapUrl);
            
        return this;
    }

    public string Build()
    {
        foreach (var sitemap in _sitemaps)
        {
            _content.AppendLine($"{Constants.Sitemap}: {sitemap}");
        }

        foreach (var userAgent in _userAgents)
        {
            _content.AppendLine($"{Constants.UserAgent}: {userAgent}");
            
            foreach (var disallow in _disallows)
            {
                _content.AppendLine($"{Constants.Disallow}: {disallow}");
            }

            foreach (var allow in _allows)
            {
                _content.AppendLine($"{Constants.Allow}: {allow}");
            }
            
            _content.AppendLine();
        }

        return _content.ToString().TrimEnd();
    }
}