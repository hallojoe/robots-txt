# RobotsTxt

A package for creating `robots.txt` files in ASP.NET Core applications. Can be used for single host as well as multihost applications.


## Features

This thing makes it easy to set up a programatically generated `robots.txt`. Create an implementation of `IRobotsTxt` and then things will wire up automatically and a `robots.txt` will be served.

The things come with the following features:

- RobotsTxtBuilder - A fluid builder
- RobotsTxtValidator - A validator

# Example

This will end up service a `robots.txt`

```
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

        // Validate example
        var robotsTxtValidator = new RobotsTxtValidator(robotsTxt);

        if (!robotsTxtValidator.IsValid) throw new InvalidDataException(nameof(robotsTxt));
        
        return robotsTxt;
    }
}

```