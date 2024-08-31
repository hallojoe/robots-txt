using System.Text.RegularExpressions;

namespace Casko.AspNetCore.RobotsTxt.Validation;

public partial class RobotsTxtValidator(string robotsTxt)
{
    [GeneratedRegex(@"^User-agent:\s*(.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex GeneratedUserAgentExpression();

    [GeneratedRegex(@"^Allow:\s*(\/.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-DK")]
    private static partial Regex GeneratedAllowExpression();

    [GeneratedRegex(@"^Disallow:\s*(\/.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-DK")]
    private static partial Regex GeneratedDisallowExpression();

    [GeneratedRegex(@"^Sitemap:\s*(https?:\/\/[^\s]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-DK")]
    private static partial Regex GeneratedSitemapExpression();

    [GeneratedRegex(@"^#.*$", RegexOptions.Compiled)]
    private static partial Regex GeneratedCommentExpression();

    public bool IsValid => ValidateRobotsTxt();

    private static readonly char[] Separator = ['\r', '\n'];

    private bool ValidateRobotsTxt()
    {
        var lines = robotsTxt.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        var userAgentDefined = false;
        var isValid = true;

        foreach (var line in lines)
        {
            if (GeneratedCommentExpression().IsMatch(line))
            {
                continue; 
            }

            if (GeneratedUserAgentExpression().IsMatch(line))
            {
                userAgentDefined = true;
                continue;
            }

            if (GeneratedAllowExpression().IsMatch(line) || GeneratedDisallowExpression().IsMatch(line))
            {
                if (!userAgentDefined)
                {
                    isValid = false; 
            
                    break;
                }

                continue;
            }

            if (GeneratedSitemapExpression().IsMatch(line))
            {
                continue;
            }

            isValid = false; 

            break;
        }

        if (!userAgentDefined)
        {
            isValid = false;
        }

        return isValid;
    }

}