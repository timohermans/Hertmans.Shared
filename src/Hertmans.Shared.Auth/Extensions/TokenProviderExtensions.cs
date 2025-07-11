using Hertmans.Shared.Auth.Services;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hertmans.Shared.Auth.Extensions;

internal static class TokenProviderExtensions
{
    public static void AddCircuitTokenProvider(this IServiceCollection services)
    {
        services.TryAddEnumerable(
            ServiceDescriptor.Scoped<CircuitHandler, TokenProvider>(sp =>
                (TokenProvider)sp.GetRequiredService<ITokenProvider>()));
        services.AddScoped<ITokenProvider, TokenProvider>();
    }
}