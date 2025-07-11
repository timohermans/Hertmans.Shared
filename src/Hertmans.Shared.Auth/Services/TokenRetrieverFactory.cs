using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hertmans.Shared.Auth.Services
{
    internal class TokenRetrieverFactory(
        IServiceProvider serviceProvider,
        string authenticationScheme)
    {
        public ITokenRetriever CreateRetriever()
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ApiBearerTokenHandler>>();
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            
            var circuitServicesAccessor = serviceProvider.GetService<CircuitServicesAccessor>();
            if (circuitServicesAccessor != null)
            {
                return new TokenProviderRetriever(circuitServicesAccessor, logger);
            }

            if (httpContextAccessor != null)
            {
                return new HttpContextTokenRetriever(authenticationScheme, httpContextAccessor, logger);
            }

            logger.LogError(
                "No token retriever is registered. the ITokenProvider or register HttpContextAccessor");
            throw new InvalidOperationException(
                $"Neither {nameof(HttpContextAccessor)} nor {nameof(ITokenProvider)} has been registered");
        }
    }
}