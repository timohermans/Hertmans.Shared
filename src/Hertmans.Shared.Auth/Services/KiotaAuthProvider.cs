using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Hertmans.Shared.Auth.Services;

public class KiotaAuthProvider(ITokenProvider tokenProvider): IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var token = tokenProvider.AccessToken;
        request.Headers.TryAdd("Authorization", "Bearer " + token);
        return Task.CompletedTask;
    }
}