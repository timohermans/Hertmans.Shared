namespace Hertmans.Shared.Auth;

public interface IApiClientOptions
{
    /// <summary>
    /// Base URL of the API
    /// </summary>
    string BaseUrl { get; set; }
    /// <summary>
    /// Authority of the authentication server
    /// </summary>
    string Authority { get; set; }
    /// <summary>
    /// Client ID of the authentication server
    /// </summary>
    string ClientId { get; set; }
    /// <summary>
    /// Client secret of the authentication server
    /// </summary>
    string ClientSecret { get; set; }
    /// <summary>
    /// Scopes of the authentication server.
    /// </summary>
    string[] Scopes { get; set; }
}