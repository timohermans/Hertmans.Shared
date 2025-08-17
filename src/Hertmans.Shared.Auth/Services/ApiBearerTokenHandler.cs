using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Hertmans.Shared.Auth.Services
{
    /// <summary>
    /// Message handler that fetches the jwt access token from possible storages 
    /// </summary>
    internal class ApiBearerTokenHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly ITokenRetriever _tokenRetriever;

        /// <summary>
        /// default ctor
        /// </summary>
        public ApiBearerTokenHandler(ILogger logger, ITokenRetriever tokenRetriever)
        {
            _logger = logger;
            _tokenRetriever = tokenRetriever;
        }

        /// <summary>
        /// Method that fires before each http call. Access token fetch magic happens here.
        /// </summary>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var accessToken = await _tokenRetriever.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException("Access token is null. Check the authentication method that the token exists.");
            }

            _logger.LogInformation("Access token successfully retrieved");

            var success = request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

            if (!success)
            {
                _logger.LogWarning("Failed to add Authorization header with Bearer token to request.");
            }
            else
            {
                _logger.LogInformation("Authorization header with Bearer token added to request.");
            }
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}