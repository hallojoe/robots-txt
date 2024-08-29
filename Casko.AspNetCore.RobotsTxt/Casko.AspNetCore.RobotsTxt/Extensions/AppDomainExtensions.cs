using System.Reflection;

namespace Casko.AspNetCore.RobotsTxt.Extensions;

internal static class AppDomainExtensions
{
    internal static Type? GetRobotsTxtServiceType(this AppDomain appDomain)
    {
        return appDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    return Type.EmptyTypes;
                }
            })
            .FirstOrDefault(type => typeof(IRobotsTxt).IsAssignableFrom(type) && type.IsClass);
    }
}