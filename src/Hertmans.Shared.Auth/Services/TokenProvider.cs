using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Hertmans.Shared.Auth.Services;

internal class TokenProvider : CircuitHandler, IDisposable, ITokenProvider
{
    private const string TokenKey = "access_token";
    private readonly PersistentComponentState _state;
    private readonly PersistingComponentStateSubscription _subscription;

    public string? AccessToken { get; private set; }

    public TokenProvider(PersistentComponentState state)
    {
        _state = state;

        if (state.TryTakeFromJson(TokenKey, out string? token))
        {
            AccessToken = token;
        }
        else
        {
            _subscription = state.RegisterOnPersisting(PersistToken);
        }
    }

    private Task PersistToken()
    {
        _state.PersistAsJson(TokenKey, AccessToken);
        return Task.CompletedTask;
    }

    public void SetToken(string token)
    {
        if (token == AccessToken)
        {
            return;
        }

        AccessToken = token;
    }

    public void Dispose() => _subscription.Dispose();
}
