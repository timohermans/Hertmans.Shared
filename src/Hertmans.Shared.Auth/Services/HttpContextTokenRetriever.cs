using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hertmans.Shared.Auth.Services
{
    internal class HttpContextTokenRetriever(
        string authenticationScheme,
        IHttpContextAccessor httpContextAccessor,
        ILogger logger)
        : ITokenRetriever
    {
        public async Task<string?> GetAccessTokenAsync()
        {
            logger.LogInformation("Trying to get access token from HttpContext");
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var auth = await httpContext.AuthenticateAsync(authenticationScheme);
                if (!auth.Succeeded)
                {
                    logger.LogError("Authenticate on httpcontext with scheme {Scheme} failed. Are you logged in?",
                        authenticationScheme);
                    throw new ArgumentException("httpcontext authenticate challenge failed on scheme: " +
                                                authenticationScheme);
                }

                return auth.Ticket.Properties.GetTokenValue("access_token");
            }

            return null;
        }
    }
}