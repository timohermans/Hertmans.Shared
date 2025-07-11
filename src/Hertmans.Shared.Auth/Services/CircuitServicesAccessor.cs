#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace Hertmans.Shared.Auth.Services
{
    internal class CircuitServicesAccessor
    {
        static readonly AsyncLocal<IServiceProvider> BlazorServices = new();

        public IServiceProvider? Services
        {
            get => BlazorServices.Value;
            set
            {
                if (value == null) return;
                BlazorServices.Value = value;
            }
        }
    }

    internal class ServicesAccessorCircuitHandler(
        IServiceProvider services,
        CircuitServicesAccessor servicesAccessor)
        : CircuitHandler
    {
        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
            Func<CircuitInboundActivityContext, Task> next) =>
            async context =>
            {
                servicesAccessor.Services = services;
                await next(context);
                servicesAccessor.Services = null;
            };
    }

    /// <summary>
    /// Extension for Blazor circuit services
    /// </summary>
    public static class CircuitServicesServiceCollectionExtensions
    {
        /// <summary>
        /// Add CircuitServicesAccessor to scope.
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>the collection</returns>
        internal static IServiceCollection AddCircuitServicesAccessor(
            this IServiceCollection services)
        {
            services.AddScoped<CircuitServicesAccessor>();
            services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();

            return services;
        }
    }
}
#endif