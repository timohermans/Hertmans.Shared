using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hertmans.Shared.Auth.Extensions;

public static class UserToApiClientExtensions
{
    /// <summary>
    /// Returns a class that can add http clients with `.AddHttpClient()`
    /// </summary>
    /// <typeparam name="T">Type of the options class that matches the section in the appsettings</typeparam>
    /// <param name="services">ServiceCollection</param>
    /// <param name="config">Configuration</param>
    /// <param name="sectionName">Section name should match a section in the appsettings.json</param>
    /// <param name="authenticationScheme">The auth scheme</param>
    /// <returns>Object that contains methods for registering the HTTP client</returns>
    public static Services.UserToApiClientExtensions.IUserToApiBuilder AddApiClientRegistration<T>(this IServiceCollection services, IConfiguration config,
        string sectionName, string authenticationScheme = "OpenIdConnect") where T : class, IApiClientOptions, new()
    {
        return Services.UserToApiClientExtensions.UserToApiBuilder.New<T>(services, config, sectionName, authenticationScheme);
    }
}