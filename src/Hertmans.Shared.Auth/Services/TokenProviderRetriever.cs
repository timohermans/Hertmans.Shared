#if NET8_0_OR_GREATER
using Microsoft.Extensions.Logging;

namespace Hertmans.Shared.Auth.Services
{
    internal class TokenProviderRetriever(CircuitServicesAccessor circuitServicesAccessor, ILogger logger)
        : ITokenRetriever
    {
        public Task<string?> GetAccessTokenAsync()
        {
            if (circuitServicesAccessor.Services?.GetService(typeof(ITokenProvider)) is ITokenProvider tokenProvider)
            {
                logger.LogInformation("Retrieved token from token provider");
                return Task.FromResult(tokenProvider.AccessToken);
            }

            return Task.FromResult<string?>(null);
        }
    }
}
#endif